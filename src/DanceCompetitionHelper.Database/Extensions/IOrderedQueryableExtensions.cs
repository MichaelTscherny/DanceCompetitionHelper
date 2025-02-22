using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Database.Extensions
{
    public static class IOrderedQueryableExtensions
    {
        public static IOrderedQueryable<Participant> ThenByDefault(
            this IOrderedQueryable<Participant> byOrderBy)
        {
            if (byOrderBy == null)
            {
                // just in case...
                return Enumerable.Empty<Participant>()
                    .AsQueryable()
                    .OrderBy(x => x.NamePartA);
            }

            return byOrderBy
                .ThenBy(
                    x => x.NamePartA)
                .ThenBy(
                    x => x.OrgIdPartA)
                .ThenBy(
                    x => x.NamePartB)
                .ThenBy(
                    x => x.OrgIdPartB)
                .ThenBy(
                    x => x.ClubName)
                .ThenBy(
                    x => x.OrgIdClub)
                .ThenBy(
                    x => x.StartNumber);
        }

        public static IOrderedAsyncEnumerable<Participant> ThenByDefault(
            this IOrderedAsyncEnumerable<Participant> byOrderBy)
        {
            if (byOrderBy == null)
            {
                // just in case...
                return AsyncEnumerable.Empty<Participant>()
                    .OrderBy(x => x.NamePartA);
            }

            return byOrderBy
                .ThenBy(
                    x => x.NamePartA)
                .ThenBy(
                    x => x.OrgIdPartA)
                .ThenBy(
                    x => x.NamePartB)
                .ThenBy(
                    x => x.OrgIdPartB)
                .ThenBy(
                    x => x.ClubName)
                .ThenBy(
                    x => x.OrgIdClub)
                .ThenBy(
                    x => x.StartNumber);
        }
    }
}
