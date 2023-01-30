using DanceCompetitionHelper.Database.Extensions;
using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Database.DisplayInfo
{
    public class MultipleStarter
    {
        public List<Participant> Participants { get; } = new List<Participant>();
        public List<CompetitionClass> CompetitionClasses { get; } = new List<CompetitionClass>();

        public string Name { get; }

        public int PointsA { get; }
        public int StartsA { get; }

        public int PointsB { get; }
        public int StartsB { get; }

        public Dictionary<Guid, int> StartnumberByClassId { get; } = new Dictionary<Guid, int>();
        public Dictionary<Guid, CompetitionClass> CompetitionClassByClassId { get; } = new Dictionary<Guid, CompetitionClass>();
        public Dictionary<Guid, string> CompetitionClassNamesByClassId { get; } = new Dictionary<Guid, string>();
        public Dictionary<string, Guid> CompetitionClassNamesByName { get; } = new Dictionary<string, Guid>();

        public MultipleStarter(
            IEnumerable<Participant> participants)
        {
            Participants.AddRange(
                participants ?? Enumerable.Empty<Participant>());

            var foundNames = new List<string>();
            foreach (var curPart in Participants)
            {
                var useCompClass = curPart.CompetitionClass;
                if (useCompClass != null
                    && CompetitionClasses.Contains(curPart.CompetitionClass) == false)
                {
                    CompetitionClasses.Add(
                        useCompClass);
                }

                foundNames.Add(
                    curPart.GetNames());

                PointsA = Math.Max(
                    PointsA,
                    curPart.OrgPointsPartA);
                StartsA = Math.Max(
                    StartsA,
                    curPart.OrgStartsPartA);

                PointsB = Math.Max(
                    PointsB,
                    curPart.OrgPointsPartB ?? 0);
                StartsB = Math.Max(
                    StartsB,
                    curPart.OrgStartsPartB ?? 0);
            }

            Name = string.Join(
                "; ",
                foundNames
                    .Select(
                        x => x?.Trim() ?? "??")
                    .Distinct());

            StartnumberByClassId = Participants.
                ToDictionary(
                    x => x.CompetitionClassId,
                    x => x.StartNumber);

            foreach (var curCompClass in CompetitionClasses)
            {
                var curCompClassId = curCompClass.CompetitionClassId;
                var curCompClassName = curCompClass.GetCompetitionClassName();

                CompetitionClassByClassId[curCompClassId] = curCompClass;
                CompetitionClassNamesByClassId[curCompClassId] = curCompClassName;
                CompetitionClassNamesByName[curCompClassName] = curCompClassId;
            }
        }

        public IEnumerable<(Guid ParticipantId, List<CompetitionClass> CompetitionClass)> GetCompetitionClassesOfParticipants()
        {
            foreach (var curPartId in Participants)
            {
                yield return (
                    curPartId.ParticipantId,
                    CompetitionClasses);
            }
        }
    }
}
