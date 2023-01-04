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

        public int? PointsB { get; }
        public int? StartsB { get; }

        public Dictionary<Guid, int> StartnumberByClassId { get; }
        public Dictionary<Guid, CompetitionClass> CompetitionClassByClassId { get; }
        public Dictionary<Guid, string> CompetitionClassNamesByClassId { get; }
        public Dictionary<string, Guid> CompetitionClassNamesByName { get; }

        public MultipleStarter(
            IEnumerable<Participant> participants)
        {
            Participants.AddRange(
                participants ?? Enumerable.Empty<Participant>());

            CompetitionClasses.AddRange(
                Participants
                    .Where(
                        x => x.CompetitionClass != null)
                    .Select(
                        x => x.CompetitionClass)
                    .Distinct());

            Name = string.Join(
                "; ",
                Participants
                .Select(
                    x => string.Format(
                        "{0} {1}",
                        x.NamePartA,
                        x.NamePartB))
                .Distinct());
            PointsA = Participants.Max(
                x => x.OrgPointsPartA);
            StartsA = Participants.Max(
                x => x.OrgStartsPartA);

            PointsB = Participants.Max(
                x => x.OrgPointsPartB);
            StartsB = Participants.Max(
                x => x.OrgStartsPartB);

            StartnumberByClassId = Participants.
                ToDictionary(
                    x => x.CompetitionClassId,
                    x => x.StartNumber);
            CompetitionClassByClassId = CompetitionClasses
                .ToDictionary(
                    x => x.CompetitionClassId,
                    x => x);
            CompetitionClassNamesByClassId = CompetitionClasses
                .ToDictionary(
                    x => x.CompetitionClassId,
                    x => x.CompetitionClassName);
            CompetitionClassNamesByName = CompetitionClasses
                .ToDictionary(
                    x => x.CompetitionClassName,
                    x => x.CompetitionClassId);
        }
    }
}
