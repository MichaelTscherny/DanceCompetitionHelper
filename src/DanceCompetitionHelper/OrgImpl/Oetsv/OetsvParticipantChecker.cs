﻿using DanceCompetitionHelper.Database.DisplayInfo;
using DanceCompetitionHelper.Database.Tables;
using Microsoft.Extensions.Logging;

namespace DanceCompetitionHelper.OrgImpl.Oetsv
{
    public class OetsvParticipantChecker : IParticipantChecker
    {
        private readonly ILogger<OetsvParticipantChecker> _logger;

        private readonly Dictionary<Guid, CompetitionClass> _compClassesById = new Dictionary<Guid, CompetitionClass>();
        private readonly Dictionary<Guid, List<CompetitionClass>> _multiStarterCompClassesByParticipantId = new Dictionary<Guid, List<CompetitionClass>>();

        private readonly ICompetitonClassChecker _competitonClassChecker;

        public OetsvParticipantChecker(
            ILogger<OetsvParticipantChecker> logger)
        {
            _logger = logger
                ?? throw new ArgumentNullException(
                        nameof(logger));

            _competitonClassChecker = new OetsvCompetitonClassChecker();
        }

        public void SetCompetitionClasses(
            IEnumerable<CompetitionClass>? competitionClasses)
        {
            foreach (var curCompClass in competitionClasses ?? Enumerable.Empty<CompetitionClass>())
            {
                _compClassesById[curCompClass.CompetitionId] = curCompClass;
            }
        }

        public void SetMultipleStarter(
            IEnumerable<MultipleStarter> multipleStarters)
        {
            foreach (var curMultiStarter in multipleStarters ?? Enumerable.Empty<MultipleStarter>())
            {
                foreach (var curPart in curMultiStarter.Participants)
                {
                    _multiStarterCompClassesByParticipantId[curPart.ParticipantId] = curMultiStarter.CompetitionClasses;
                }
            }
        }

        public CheckPromotionInfo CheckParticipantPromotion(
            Participant participant)
        {
            var usePartCompClass = participant.CompetitionClass;
            var allClasses = new List<CompetitionClass>();

            if (_multiStarterCompClassesByParticipantId.TryGetValue(
                participant.ParticipantId,
                out var foundAllStartingClasses))
            {
                allClasses.AddRange(
                    _competitonClassChecker.GetRelatedCompetitionClassesForPoints(
                        usePartCompClass,
                        foundAllStartingClasses));
            }

            if (allClasses.Contains(
                usePartCompClass) == false)
            {
                allClasses.Add(
                    usePartCompClass);
            }

            var retChkPromo = GetCheckPromotionInfo(
                participant,
                allClasses);

            // check all follow-up classes till "promotion"
            // is reached...
            foreach (var curClass in allClasses.ToList())
            {
                var useFollowUpClass = curClass.FollowUpCompetitionClass;

                if (retChkPromo.PossiblePromotion)
                {
                    return retChkPromo;
                }

                // check all follow-ups...
                while (useFollowUpClass != null)
                {
                    if (useFollowUpClass.Ignore == true)
                    {
                        break;
                    }

                    if (allClasses.Contains(
                        useFollowUpClass) == false)
                    {
                        allClasses.Add(
                            useFollowUpClass);
                    }

                    // must not be me...
                    if (useFollowUpClass.FollowUpCompetitionClass != useFollowUpClass)
                    {
                        useFollowUpClass = useFollowUpClass.FollowUpCompetitionClass;
                    }
                    else
                    {
                        useFollowUpClass = null;
                    }

                    retChkPromo = GetCheckPromotionInfo(
                        participant,
                        allClasses);

                    if (retChkPromo.PossiblePromotion)
                    {
                        return retChkPromo;
                    }
                }
            }

            return retChkPromo;
        }

        public CheckPromotionInfo GetCheckPromotionInfo(
            Participant participant,
            List<CompetitionClass> allClasses)
        {
            var usePartCompClass = participant.CompetitionClass;

            // let's check...
            var pointsForFirst = allClasses
                .Where(
                    x => x.PointsForFirst != OetsvConstants.Classes.NoPromotionPossible)
                .Sum(
                    x => x.PointsForFirst);
            var countStarts = allClasses.Count;

            var newPartAPoints = (participant.OrgPointsPartA + pointsForFirst);
            var promotionPartAPoints = usePartCompClass.MinPointsForPromotion > 0
                && newPartAPoints >= usePartCompClass.MinPointsForPromotion;
            double? newPartBPoints = ((participant.OrgPointsPartB ?? 0) + pointsForFirst);
            bool? promotionPartBPoints = usePartCompClass.MinPointsForPromotion > 0
                && newPartBPoints >= usePartCompClass.MinPointsForPromotion;
            var (alreadyPromotedA, alreadyPromotedAInfo) = CheckAlreadyPromoted(
                participant.OrgAlreadyPromotedPartA,
                participant.OrgPointsPartA,
                participant.OrgStartsPartA,
                participant.CompetitionClass.MinPointsForPromotion,
                participant.CompetitionClass.MinStartsForPromotion);

            var newPartAStarts = (participant.OrgStartsPartA + countStarts);
            var useMinStartsForPromotionPartA = (participant.MinStartsForPromotionPartA ?? usePartCompClass.MinStartsForPromotion);
            var promotionPartAStarts = useMinStartsForPromotionPartA > 0
                && newPartAStarts >= useMinStartsForPromotionPartA;
            int? newPartBStarts = ((participant.OrgStartsPartB ?? 0) + countStarts);
            var useMinStartsForPromotionPartB = (participant.MinStartsForPromotionPartB ?? usePartCompClass.MinStartsForPromotion);
            bool? promotionPartBStarts = useMinStartsForPromotionPartB > 0
                && newPartBStarts >= useMinStartsForPromotionPartB;
            bool? alreadyPromotedB = null;
            string? alreadyPromotedBInfo = null;

            // AND NOW?..
            var retPromotionA = (participant.OrgAlreadyPromotedPartA ?? false)
                || (promotionPartAPoints
                && promotionPartAStarts);

            bool? retPromotionB = null;

            // check already promoted
            if (participant.OrgPointsPartB.HasValue)
            {
                retPromotionB = (participant.OrgAlreadyPromotedPartB ?? false)
                    || (promotionPartBPoints.Value
                    && promotionPartBStarts.Value);

                (alreadyPromotedB, alreadyPromotedBInfo) = CheckAlreadyPromoted(
                    participant.OrgAlreadyPromotedPartB,
                    participant.OrgPointsPartB,
                    participant.OrgStartsPartB,
                    participant.CompetitionClass.MinPointsForPromotion,
                    participant.CompetitionClass.MinStartsForPromotion);
            }
            else
            {
                newPartBPoints = null;
                promotionPartBPoints = null;
                newPartBStarts = null;
                promotionPartBStarts = null;
            }

            _logger.LogDebug(
                "Participant '{NamePartA}'/'{NamePartB}' ({ParticipantId}): P/S " +
                "[A] {CurrentOrgPointsPartA}/{CurrentOrgStartsPartA}; " +
                "[B] {CurrentOrgPointsPartB}/{CurrentOrgStartsPartB} + " +
                "F/S {pointsForFirst}/{countStarts} = " +
                "S/P [A] {newPartAPoints}/{newPartAStarts}; " +
                "[B] {newPartBPoints}/{newPartBPoints} => " +
                "Prom S/P [A] {promotionPartAPoints}/{promotionPartAStarts}; " +
                "[B] {promotionPartBPoints}/{promotionPartBStarts} -> " +
                "[A/B] ({retPromotionA}/{retPromotionB}) " +
                "('{CompetitionClassName}': Min S/P {MinPointsForPromotion}/{MinStartsForPromotion} ([A/B] {MinStartsForPromotionPartA}/{MinStartsForPromotionPartB}))",
                participant.NamePartA,
                participant.NamePartB,
                participant.ParticipantId,
                participant.OrgPointsPartA,
                participant.OrgStartsPartA,
                participant.OrgPointsPartB,
                participant.OrgStartsPartB,
                pointsForFirst,
                countStarts,
                newPartAPoints,
                newPartAStarts,
                newPartBPoints,
                newPartBStarts,
                promotionPartAPoints,
                promotionPartAStarts,
                promotionPartBPoints,
                promotionPartBStarts,
                retPromotionA,
                retPromotionB,
                usePartCompClass.CompetitionClassName,
                usePartCompClass.MinPointsForPromotion,
                usePartCompClass.MinStartsForPromotion,
                participant.MinStartsForPromotionPartA,
                participant.MinStartsForPromotionPartB);

            return new CheckPromotionInfo()
            {
                // A
                PossiblePromotionA = retPromotionA,
                PossiblePromotionAInfo = string.Format(
                    "[A] {0}/{1} + {2}/{3} = {4}/{5} -> {6}",
                    participant.OrgPointsPartA,
                    participant.OrgStartsPartA,
                    pointsForFirst,
                    countStarts,
                    newPartAPoints,
                    newPartAStarts,
                    retPromotionA),
                AlreadyPromotionA = alreadyPromotedA,
                AlreadyPromotionAInfo = alreadyPromotedAInfo,
                // B
                PossiblePromotionB = retPromotionB,
                PossiblePromotionBInfo = participant.OrgPointsPartB.HasValue
                    ? string.Format(
                        "[B] {0}/{1} + {2}/{3} = {4}/{5} -> {6}",
                        participant.OrgPointsPartB,
                        participant.OrgStartsPartB,
                        pointsForFirst,
                        countStarts,
                        newPartBPoints,
                        newPartBStarts,
                        retPromotionB)
                    : null,
                AlreadyPromotionB = alreadyPromotedB,
                AlreadyPromotionBInfo = alreadyPromotedBInfo,

                IncludedCompetitionClasses = allClasses,
            };
        }

        public (bool? AlreadyPromoted, string? AlreadyPromotedInfo) CheckAlreadyPromoted(
            bool? orgAlreadyPromoted,
            double? curOrgPoints,
            int? curOrgStarts,
            double minPointsForPromotion,
            int minStartsForPromotion)
        {
            if (orgAlreadyPromoted ?? false)
            {
                return (
                    true,
                    "Already Promted by Org");
            }

            if (minPointsForPromotion > 0
                && minStartsForPromotion > 0
                && curOrgPoints >= minPointsForPromotion
                && curOrgStarts >= minStartsForPromotion)
            {
                return (
                    true,
                    string.Format(
                    "{0}/{1} >= {2}/{3}",
                    curOrgPoints,
                    curOrgStarts,
                    minPointsForPromotion,
                    minStartsForPromotion));
            }

            return (
                null,
                null);
        }
    }
}
