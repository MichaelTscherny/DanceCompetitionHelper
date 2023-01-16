using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.OrgImpl.Oetsv
{
    public class OetsvCompetitonClassChecker : ICompetitonClassChecker
    {


        public IEnumerable<CompetitionClass> GetRelatedCompetitionClassesForPoints(
            CompetitionClass baseCompetitionClass,
            IEnumerable<CompetitionClass> checkCompetitionClasses)
        {
            if (baseCompetitionClass == null
                || checkCompetitionClasses == null)
            {
                yield break;
            }

            foreach (var curChkClass in checkCompetitionClasses)
            {
                if (curChkClass.Discipline != baseCompetitionClass.Discipline)
                {
                    continue;
                }

                if (OetsvConstants.Points.AgeClassForSamePoints(
                    baseCompetitionClass.AgeClass,
                    curChkClass.AgeClass) == false)
                {
                    continue;
                }

                yield return curChkClass;
            }
        }
    }
}
