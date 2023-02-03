using DanceCompetitionHelper.Database.DisplayInfo;
using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Web.Models
{
    public class ShowPossiblePromotionsViewModel
    {
        public Competition? Competition { get; }
        public List<MultipleStarter> MultipleStarters { get; } = new List<MultipleStarter>();
        public List<Participant> PossiblePromotions { get; } = new List<Participant>();

        // TODO: needed? or are filtered "PossiblePromotions" all we need?..
        public List<ParticipantPromotionInfo> ParticipantPromotionInfo { get; } = new List<ParticipantPromotionInfo>();

        public ShowPossiblePromotionsViewModel(
            Competition? competition,
            IEnumerable<Participant>? possiblePromotions,
            IEnumerable<MultipleStarter>? multipleStarters)
        {
            Competition = competition;
            PossiblePromotions.AddRange(
                possiblePromotions?.Where(
                    x => x.DisplayInfo != null
                    && x.DisplayInfo.PromotionInfo != null
                    && x.DisplayInfo.PromotionInfo.PossiblePromotion)
                ?? Enumerable.Empty<Participant>());

            MultipleStarters.AddRange(
                multipleStarters ?? Enumerable.Empty<MultipleStarter>());

            MergeData();
        }

        public void MergeData()
        {
            ParticipantPromotionInfo.Clear();
            var partPromInfoByPartId = new Dictionary<Guid, ParticipantPromotionInfo>();

            foreach (var curMultiStarter in MultipleStarters)
            {
                var newPartPromInfo = new ParticipantPromotionInfo();

                foreach (var curPart in curMultiStarter.Participants)
                {
                    newPartPromInfo.AddParticipant(
                        curPart);

                    partPromInfoByPartId[curPart.ParticipantId] = newPartPromInfo;
                }

                foreach (var curCompClass in curMultiStarter.CompetitionClasses)
                {
                    newPartPromInfo.AddCompetitionClass(
                        curCompClass);
                }
            }

            var validPosPromotionsByParticipantsId = new HashSet<Guid>();
            foreach (var curPosPromPart in PossiblePromotions)
            {
                var useParticipantId = curPosPromPart.ParticipantId;
                validPosPromotionsByParticipantsId.Add(
                    useParticipantId);

                if (partPromInfoByPartId.TryGetValue(
                    useParticipantId,
                    out var foundPromInfo))
                {
                    foundPromInfo.AddCompetitionClass(
                        curPosPromPart.CompetitionClass);
                }
                else
                {
                    var newPartPromInfo = new ParticipantPromotionInfo();
                    newPartPromInfo.AddParticipant(
                        curPosPromPart);
                    newPartPromInfo.AddCompetitionClass(
                        curPosPromPart.CompetitionClass);

                    partPromInfoByPartId[useParticipantId] = newPartPromInfo;
                }
            }

            var toAdd = new List<ParticipantPromotionInfo>();
            foreach (var promInfo in partPromInfoByPartId.Values)
            {

            }

            ParticipantPromotionInfo.AddRange(
                partPromInfoByPartId
                    .Select(
                        x => x.Value)
                    .Where(
                        x => x.IsSameParticipant(
                            validPosPromotionsByParticipantsId))
                    .Distinct());
        }
    }
}
