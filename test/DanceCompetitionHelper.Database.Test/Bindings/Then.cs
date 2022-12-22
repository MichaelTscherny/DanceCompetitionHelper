using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Database.Test.Pocos.DanceCompetitionHelper;
using NuGet.Frameworks;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using TestHelper.Extensions;

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

            foreach (var toChk in checkComps)
            {
                var foundComp = useDb.Competitions.FirstOrDefault(
                    x => x.CompetitionName == toChk.CompetitionName);
                Assert.NotNull(
                    foundComp,
                    "{0} {1} not found!",
                    nameof(Competition),
                    toChk);

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
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            DisposeAllCreated();
        }
    }
}
