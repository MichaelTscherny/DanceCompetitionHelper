using DanceCompetitionHelper.Database.DisplayInfo;
using DanceCompetitionHelper.Database.Extensions;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Extensions;
using DanceCompetitionHelper.Web.Enums;

namespace DanceCompetitionHelper.Web.Extensions
{
    public static class ViewModelExtensions
    {
        public static List<List<(string ClassName, Guid ClassId, bool Ignore)>> ExtractDisplayInfo(
            IEnumerable<MultipleStarter>? multipleStarters,
            GroupForViewEnum groupForView = GroupForViewEnum.None)
        {
            return ExtractDisplayInfo(
                multipleStarters
                    ?.SelectMany(
                        x => x.CompetitionClasses),
                groupForView);
        }

        public static List<List<(string ClassName, Guid ClassId, bool Ignore)>> ExtractDisplayInfo(
            IEnumerable<CompetitionClass>? competitionClasses,
            GroupForViewEnum groupForView = GroupForViewEnum.None)
        {
            var classesByDiscipline = new Dictionary<string, List<CompetitionClass>>();
            foreach (var curCompClass in competitionClasses
                ?.OrderBy(
                    x => x.OrgClassId)
                ?? Enumerable.Empty<CompetitionClass>())
            {
                switch (groupForView)
                {
                    case GroupForViewEnum.None:
                        classesByDiscipline.AddToBucket(
                            "nameof(classesByDiscipline)",
                            curCompClass);
                        break;

                    case GroupForViewEnum.ByDisciplineAndOrgClassId:
                        classesByDiscipline.AddToBucket(
                            curCompClass.Discipline ?? "xxxxx",
                            curCompClass);
                        break;
                }
            }

            var retDisplayInfosGrouped = new List<List<(string ClassName, Guid ClassId, bool Ignore)>>();

            foreach (var curClassByDisp in classesByDiscipline.OrderBy(
                x => x.Key))
            {
                retDisplayInfosGrouped.Add(
                    new List<(string ClassName, Guid ClassId, bool Ignore)>(
                        curClassByDisp.Value
                            .OrderBy(
                                x => x.OrgClassId)
                            .Select(
                                x => (x.GetCompetitionClassName(),
                                    x.CompetitionClassId,
                                    x.Ignore))
                            .Distinct()));
            }

            return retDisplayInfosGrouped;
        }

        public static (List<(HashSet<Guid> LinkedClasses, List<MultipleStarter> MultipleStarters)> LinkedClassIdsAndMultipleStarters, List<List<(string ClassName, Guid ClassId, bool Ignore)>> DisplayInfos) ExtractMultipleStarterDependentClasses(
            IEnumerable<MultipleStarter>? multipleStarters,
            GroupForViewEnum groupForView = GroupForViewEnum.None)
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
                    multipleStarters,
                    groupForView));
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
