using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Web.Enums
{
    public enum GroupForViewEnum
    {
        /// <summary>
        /// Does not group but sort by <see cref="CompetitionClass.OrgClassId"/>
        /// </summary>
        None = 0,

        /// <summary>
        /// Separates all classes by <see cref="CompetitionClass.Discipline"/>
        /// and orders it by <see cref="CompetitionClass.OrgClassId"/>
        /// </summary>
        ByDisciplineAndOrgClassId,
    }
}
