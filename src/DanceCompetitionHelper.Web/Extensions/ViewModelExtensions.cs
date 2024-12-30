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

        public static (List<(HashSet<Guid> LinkedClasses, List<MultipleStarter> MultipleStarters)> LinkedClassIdsAndMultipleStarters, List<(string ClassName, Guid ClassId, bool Ignore)> DisplayInfo) ExtractMultipleStarterLinkedClasses(
            IEnumerable<MultipleStarter>? multipleStarters)
        {
            var linkedClassIds = new List<HashSet<Guid>>();
            var classesByIds = new Dictionary<Guid, CompetitionClass>();

            var useMultipleStarters = (multipleStarters ?? Enumerable.Empty<MultipleStarter>());
            foreach (var curStarter in useMultipleStarters)
            {
                foreach (var (_, partClasses) in curStarter.GetCompetitionClassesOfParticipants())
                {
                    var partClassesIds = new HashSet<Guid>();

                    foreach (var curClass in partClasses)
                    {
                        var useCompClassId = curClass.CompetitionClassId;

                        if (partClassesIds.Contains(
                            useCompClassId))
                        {
                            continue;
                        }

                        partClassesIds.Add(
                            useCompClassId);
                        classesByIds[useCompClassId] = curClass;
                    }

                    var foundSet = false;
                    foreach (var curSet in linkedClassIds)
                    {
                        if (curSet
                            .Intersect(
                                partClassesIds)
                            .Any())
                        {
                            curSet.UnionWith(
                                partClassesIds);
                            foundSet = true;
                            break;
                        }
                    }

                    if (foundSet == false)
                    {
                        linkedClassIds.Add(
                            partClassesIds);
                        continue;
                    }
                }
            }

            var useLinkedClassIds = new List<HashSet<Guid>>();

            // cleanup!..
            // extra merge to avoid "later linked"
            foreach (var curLinkedIds in linkedClassIds)
            {
                var foundSet = false;
                foreach (var curSet in useLinkedClassIds)
                {
                    if (curSet
                        .Intersect(
                            curLinkedIds)
                        .Any())
                    {
                        curSet.UnionWith(
                            curLinkedIds);
                        foundSet = true;
                        break;
                    }
                }

                if (foundSet == false)
                {
                    useLinkedClassIds.Add(
                        curLinkedIds);
                    continue;
                }
            }

            // link participants
            var retLinkedClassIdsAndMultStarters = new List<(HashSet<Guid> LinkedClasses, List<MultipleStarter> MultipleStarters)>();

            foreach (var curClassIds in useLinkedClassIds)
            {
                retLinkedClassIdsAndMultStarters.Add(
                    (curClassIds,
                    useMultipleStarters
                        .Where(
                            x => curClassIds
                                .Intersect(
                                    x.CompetitionClassByClassId.Keys)
                                .Any())
                        .ToList()));
            }

            return (
                retLinkedClassIdsAndMultStarters,
                ExtractDisplayInfo(
                    multipleStarters));
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
