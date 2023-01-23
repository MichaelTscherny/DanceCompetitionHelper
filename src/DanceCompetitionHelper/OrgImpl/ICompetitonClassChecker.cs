using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.OrgImpl
{
    public interface ICompetitonClassChecker
    {
        public IEnumerable<CompetitionClass> GetRelatedCompetitionClassesForPoints(
            CompetitionClass baseCompetitionClass,
            IEnumerable<CompetitionClass> checkCompetitionClasses);

        public IEnumerable<CompetitionClass> GetHigherClassifications(
            CompetitionClass forCompetitionClass,
            IEnumerable<CompetitionClass> checkCompetitionClasses);
    }
}
