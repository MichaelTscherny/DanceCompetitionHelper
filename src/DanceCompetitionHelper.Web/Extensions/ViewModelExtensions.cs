using DanceCompetitionHelper.Database.DisplayInfo;
using DanceCompetitionHelper.Database.Extensions;
using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Web.Extensions
{
    public static class ViewModelExtensions
    {
        public static List<(string ClassName, Guid ClassId, bool Ignore)> ExtractDisplayInfo(
            IEnumerable<MultipleStarter>? multipleStarters)
        {
            return ExtractDisplayInfo(
                multipleStarters
                    ?.SelectMany(
                        x => x.CompetitionClasses));
        }

        public static List<(string ClassName, Guid ClassId, bool Ignore)> ExtractDisplayInfo(
            IEnumerable<CompetitionClass>? multipleStarters)
        {
            return new List<(string ClassName, Guid ClassId, bool Ignore)>(
                multipleStarters
                    ?.OrderBy(
                        x => x.OrgClassId)
                    .Select(
                        x => (x.GetCompetitionClassName(),
                            x.CompetitionClassId,
                            x.Ignore))
                    .Distinct()
                    ?? Enumerable.Empty<(string, Guid, bool)>());
        }

        public static (List<CompetitionClass> IncludedClasses, int MaxDisplayClasses) ExtractPossiblePromotionCompetitionClasses(
            IEnumerable<Participant>? participants)
        {
            var helpIncludedClasses = new List<CompetitionClass>();
            var maxDisplayClasses = 0;

            foreach (var curPossibleProm in participants
                ?.Where(
                    x => x.DisplayInfo != null
                    && x.DisplayInfo.PromotionInfo != null)
                ?? Enumerable.Empty<Participant>())
            {
                var curClasses = curPossibleProm?.DisplayInfo?.PromotionInfo?.IncludedCompetitionClasses
                    ?? new List<CompetitionClass>();

                maxDisplayClasses = Math.Max(
                    maxDisplayClasses,
                    curClasses.Count);

                helpIncludedClasses.AddRange(
                    curClasses);
            }

            return (helpIncludedClasses,
                maxDisplayClasses);
        }
    }
}
