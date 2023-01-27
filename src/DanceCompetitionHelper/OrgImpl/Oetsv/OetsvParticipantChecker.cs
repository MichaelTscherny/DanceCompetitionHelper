using DanceCompetitionHelper.Database.DisplayInfo;
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

            var myCompClassChecker = new OetsvCompetitonClassChecker();

            _competitonClassChecker = myCompClassChecker;
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

        public CheckPromotionInfo CheckParticipantPromotion(Participant participant)
        {
            var allClasses = new List<CompetitionClass>();
            var usePartCompClass = participant.CompetitionClass;

            if (_multiStarterCompClassesByParticipantId.TryGetValue(
                participant.ParticipantId,
                out var foundAllStartingClasses))
            {
                allClasses.AddRange(
                    _competitonClassChecker.GetRelatedCompetitionClassesForPoints(
                        usePartCompClass,
                        foundAllStartingClasses));
            }
            else
            {
                allClasses.Add(
                    usePartCompClass);
            }

            var pointsForFirst = allClasses.Sum(x => x.PointsForFirst);
            var countStarts = allClasses.Count;

            var newPartAPoints = (participant.OrgPointsPartA + pointsForFirst);
            var promotionPartAPoints = newPartAPoints >= usePartCompClass.MinPointsForPromotion;
            int? newPartBPoints = ((participant.OrgPointsPartB ?? 0) + pointsForFirst);
            bool? promotionPartBPoints = newPartBPoints >= usePartCompClass.MinPointsForPromotion;

            var newPartAStarts = (participant.OrgStartsPartA + countStarts);
            var promotionPartAStarts = newPartAStarts >= (participant.MinStartsForPromotionPartA ?? usePartCompClass.MinStartsForPromotion);
            int? newPartBStarts = ((participant.OrgStartsPartB ?? 0) + countStarts);
            bool? promotionPartBStarts = newPartBStarts >= (participant.MinStartsForPromotionPartB ?? usePartCompClass.MinStartsForPromotion);

            var retPromotionA = promotionPartAPoints
                && promotionPartAStarts;

            bool? retPromotionB = null;

            if (participant.OrgPointsPartB.HasValue)
            {
                retPromotionB = promotionPartBPoints.Value
                    && promotionPartBStarts.Value;
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
                IncludedCompetitionClasses = allClasses,
            };
        }
    }
}
