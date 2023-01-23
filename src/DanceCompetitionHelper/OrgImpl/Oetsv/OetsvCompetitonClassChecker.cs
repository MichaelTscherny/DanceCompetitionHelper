using DanceCompetitionHelper.Database.Tables;
using static DanceCompetitionHelper.OrgImpl.Oetsv.OetsvConstants;

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

        public CompetitionClass? GetHigherClassifications(
            CompetitionClass forCompetitionClass,
            IEnumerable<CompetitionClass> checkCompetitionClasses)
        {
            if (forCompetitionClass == null
                || checkCompetitionClasses == null)
            {
                return null;
            }

            var higherCompClass = Classes.GetHigherClassifications(
                forCompetitionClass.Class);

            foreach (var curChkClass in checkCompetitionClasses)
            {
                if (curChkClass.CompetitionClassId == forCompetitionClass.CompetitionClassId
                    && curChkClass.Discipline == forCompetitionClass.Discipline
                    && curChkClass.AgeClass == forCompetitionClass.AgeClass
                    && curChkClass.Class == higherCompClass)
                {
                    return curChkClass;
                }
            }

            return null;

        }
    }
}
