using DanceCompetitionHelper.Database.Tables;
using static DanceCompetitionHelper.OrgImpl.Oetsv.OetsvConstants;

namespace DanceCompetitionHelper.OrgImpl.Oetsv
{
    public class OetsvCompetitonClassChecker : ICompetitonClassChecker
    {
        /*
        private readonly ILogger<OetsvCompetitonClassChecker> _logger;

        public OetsvCompetitonClassChecker(
            ILogger<OetsvCompetitonClassChecker> logger)
        {
            _logger = logger
                ?? throw new ArgumentNullException(
                    nameof(logger));
        }
        */

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

        public IEnumerable<CompetitionClass> GetHigherClassifications(
            CompetitionClass forCompetitionClass,
            IEnumerable<CompetitionClass> checkCompetitionClasses)
        {
            if (forCompetitionClass == null
                || checkCompetitionClasses == null)
            {
                yield break;
            }

            var higherCompClass = Classes.GetHigherClassifications(
                forCompetitionClass.Class);

            foreach (var curChkClass in checkCompetitionClasses)
            {
                if (curChkClass.CompetitionId == forCompetitionClass.CompetitionId
                    && curChkClass.Discipline == forCompetitionClass.Discipline
                    && curChkClass.AgeClass == forCompetitionClass.AgeClass
                    && curChkClass.AgeGroup == forCompetitionClass.AgeGroup
                    && curChkClass.Class == higherCompClass)
                {
                    yield return curChkClass;
                }
            }
        }
    }
}
