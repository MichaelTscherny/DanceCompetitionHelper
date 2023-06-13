using DanceCompetitionHelper.Database.Enum;
using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Database.Extensions
{
    public static class IOrderedQueryableExtensions
    {
        public static IOrderedEnumerable<Participant> OrderByOrganization(
            this IEnumerable<Participant> byOrderBy,
            OrganizationEnum organization)
        {
            if (byOrderBy == null)
            {
                // just in case...
                return Enumerable.Empty<Participant>()
                    // .AsQueryable()
                    .OrderBy(x => x.NamePartA);
            }

            // better sorts...
            switch (organization)
            {
                case OrganizationEnum.Oetsv:
                    return byOrderBy
                        .OrderBy(
                            x => TryParseOrgClassId(
                                x.CompetitionClass.OrgClassId));

                default:
                    return byOrderBy
                        .OrderBy(
                            x => x.CompetitionClass.OrgClassId);
            }
        }

        public static int TryParseOrgClassId(
            string orgClassId)
        {
            if (int.TryParse(
                orgClassId,
                out var parsed))
            {
                return parsed;
            }

            return 0;
        }

        public static IOrderedEnumerable<CompetitionClass> OrderByOrganization(
            this IEnumerable<CompetitionClass> byOrderBy,
            OrganizationEnum organization)
        {
            if (byOrderBy == null)
            {
                // just in case...
                return Enumerable.Empty<CompetitionClass>()
                    // .AsQueryable()
                    .OrderBy(x => x.OrgClassId);
            }

            // better sorts...
            switch (organization)
            {
                case OrganizationEnum.Oetsv:
                    return byOrderBy
                        .OrderBy(
                            x => TryParseOrgClassId(
                                x.OrgClassId));

                default:
                    return byOrderBy
                        .OrderBy(
                            x => x.OrgClassId);
            }
        }

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

        public static IOrderedEnumerable<Participant> ThenByDefault(
            this IOrderedEnumerable<Participant> byOrderBy)
        {
            if (byOrderBy == null)
            {
                // just in case...
                return Enumerable.Empty<Participant>()
                    // .AsQueryable()
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
