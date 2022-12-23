using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Database.Test.Pocos.DanceCompetitionHelper;
using NUnit.Framework;
using TechTalk.SpecFlow.Assist;

namespace DanceCompetitionHelper.Database.Test.Bindings
{
    [Binding]
    public sealed class Then : BindingBase
    {
        public Then(
            ScenarioContext scenarioContext)
            : base(
                  scenarioContext)
        {
        }

        [Then(@"following Competitions exists in ""([^""]*)""")]
        public void ThenFollowingCompetitionsExistsIn(
            string danceCompHelperDb,
            Table table)
        {
            var checkComps = table.CreateSet<CompetitionPoco>();
            var useDb = GetDanceCompetitionHelperDbContext(
                danceCompHelperDb);

            using var dbTrans = useDb.BeginTransaction();

            try
            {
                foreach (var toChk in checkComps)
                {
                    var foundComp = useDb.Competitions.FirstOrDefault(
                        x => x.CompetitionName == toChk.CompetitionName);

                    Assert.That(
                        foundComp,
                        Is.Not.Null,
                        "{0} '{1}' not found!",
                        nameof(Competition),
                        toChk);

                    Assert.Multiple(() =>
                    {
                        Assert.That(
                            foundComp.CompetitionName,
                            Is.EqualTo(
                                toChk.CompetitionName),
                            "{0}: {1}",
                            toChk,
                            nameof(foundComp.CompetitionName));
                        Assert.That(
                            foundComp.Organization,
                            Is.EqualTo(
                                toChk.Organization),
                            "{0}: {1}",
                            toChk,
                            nameof(foundComp.Organization));
                        Assert.That(
                            foundComp.OrgCompetitionId,
                            Is.EqualTo(
                                toChk.OrgCompetitionId),
                            "{0}: {1}",
                            toChk,
                            nameof(foundComp.OrgCompetitionId));
                        Assert.That(
                            foundComp.CompetitionInfo,
                            Is.EqualTo(
                                toChk.CompetitionInfo),
                            "{0}: {1}",
                            toChk,
                            nameof(foundComp.CompetitionInfo));
                        Assert.That(
                            foundComp.CompetitionDate,
                            Is.EqualTo(
                                toChk.CompetitionDate ?? UseNow),
                            "{0}: {1}",
                            toChk,
                            nameof(foundComp.CompetitionDate));
                    });
                }
            }
            finally
            {
                dbTrans.Rollback();
            }
        }

        [Then(@"following Competition Classes exists in ""([^""]*)""")]
        public void ThenFollowingCompetitionClassesExistsIn(
            string danceCompHelperDb,
            Table table)
        {
            var checkCompClassess = table.CreateSet<CompetitionClassPoco>();
            var useDb = GetDanceCompetitionHelperDbContext(
                danceCompHelperDb);

            using var dbTrans = useDb.BeginTransaction();
            try
            {
                foreach (var curChk in checkCompClassess)
                {
                    var foundComp = useDb.Competitions.FirstOrDefault(
                        x => x.CompetitionName == curChk.CompetitionName);

                    Assert.That(
                        foundComp,
                        Is.Not.Null,
                        "{0} '{1}' not found!",
                        nameof(Competition),
                        curChk);

                    var foundCompClass = useDb.CompetitionClasses.FirstOrDefault(
                        x => x.Competition == foundComp
                        && x.OrgClassId == curChk.OrgClassId);

                    Assert.That(
                        foundCompClass,
                        Is.Not.Null,
                        "{0} '{1}' not found!",
                        nameof(CompetitionClass),
                        curChk);

                    Assert.Multiple(() =>
                    {
                        Assert.That(
                            foundCompClass.CompetitionId,
                            Is.EqualTo(
                                foundComp.CompetitionId),
                            "{0}: {1}",
                            curChk,
                            nameof(foundComp.CompetitionId));
                        Assert.That(
                            foundCompClass.Version,
                            Is.EqualTo(
                                curChk.Version),
                            "{0}: {1}",
                            curChk,
                            nameof(curChk.Version));
                        Assert.That(
                            foundCompClass.OrgClassId,
                            Is.EqualTo(
                                curChk.OrgClassId),
                            "{0}: {1}",
                            curChk,
                            nameof(curChk.OrgClassId));
                        Assert.That(
                            foundCompClass.Discipline,
                            Is.EqualTo(
                                curChk.Discipline),
                            "{0}: {1}",
                            curChk,
                            nameof(curChk.Discipline));
                        Assert.That(
                            foundCompClass.AgeClass,
                            Is.EqualTo(
                                curChk.AgeClass),
                            "{0}: {1}",
                            curChk,
                            nameof(curChk.AgeClass));
                        Assert.That(
                            foundCompClass.AgeGroup,
                            Is.EqualTo(
                                curChk.AgeGroup),
                            "{0}: {1}",
                            curChk,
                            nameof(curChk.AgeGroup));
                        Assert.That(
                            foundCompClass.Class,
                            Is.EqualTo(
                                curChk.Class),
                            "{0}: {1}",
                            curChk,
                            nameof(curChk.Class));

                        Assert.That(
                            foundCompClass.MinStartsForPromotion,
                            Is.EqualTo(
                                curChk.MinStartsForPromotion),
                            "{0}: {1}",
                            curChk,
                            nameof(curChk.MinStartsForPromotion));
                        Assert.That(
                            foundCompClass.MinPointsForPromotion,
                            Is.EqualTo(
                                curChk.MinPointsForPromotion),
                            "{0}: {1}",
                            curChk,
                            nameof(curChk.MinPointsForPromotion));
                    });
                }
            }
            finally
            {
                dbTrans.Rollback();
            }
        }


        [AfterScenario]
        public void AfterScenario()
        {
            DisposeAllCreated();
        }
    }
}
