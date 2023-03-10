using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Database.Extensions
{
    public static class HumanReadableExtensions
    {
        public static string GetCompetitionName(
            this Competition? forCompetition) =>
                string.Format(
                    "{0} ({1})",
                    forCompetition?.CompetitionName ?? "??",
                    forCompetition?.OrgCompetitionId ?? "??");

        public static string GetCompetitionClassName(
            this CompetitionClass? forCompetitionClass) =>
                string.Format(
                    "{0} ({1})",
                    forCompetitionClass?.CompetitionClassName ?? "??",
                    forCompetitionClass?.OrgClassId ?? "??");

        public static string GetCompetitionClasseNames(
            this IEnumerable<CompetitionClass>? forCompetitionClasses) =>
                string.Join(
                    "; ",
                    (forCompetitionClasses ?? Enumerable.Empty<CompetitionClass>())
                        .OrderBy(
                            x => x.OrgClassId)
                        .Select(
                            x => x.GetCompetitionClassName()));

        public static string GetStartNumber(
            this IEnumerable<Participant>? forParticipants) =>
                string.Join(
                    ", ",
                    forParticipants
                        ?.Select(
                            x => x.StartNumber)
                        ?.OrderBy(
                            x => x)
                        ?.Select(
                            x => string.Format(
                                "#{0}",
                                x))
                        ?? Enumerable.Empty<string>());

        public static string GetNames(
            this Participant? forParticipant) =>
                string.Format(
                        "{0} {1}",
                        forParticipant?.NamePartA ?? string.Empty,
                        forParticipant?.NamePartB ?? string.Empty);

    }
}
