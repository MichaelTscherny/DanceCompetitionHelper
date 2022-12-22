using DanceCompetitionHelper.Database.Test.Pocos;
using DanceCompetitionHelper.Database.Test.Pocos.DanceCompetitionHelper;
using Microsoft.EntityFrameworkCore;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using TestHelper.Extensions;

namespace DanceCompetitionHelper.Database.Test.Bindings
{
    [Binding]
    public sealed class Given : BindingBase
    {
        public Given(
            ScenarioContext scenarioContext)
            : base(
                  scenarioContext)
        {
        }

        [Given(@"following DanceComp-DB ""([^""]*)""")]
        public void GivenFollowingDanceCompDb(
            string danceCompHelperDb)
        {
            var newDbFile = string.Format(
                "{0}_{1}.sqlite",
                UseNow.ToString("yyyyMMdd_HHmmss"),
                danceCompHelperDb);

            Console.WriteLine(
                "Create Test-DB '{0}' -> {1}",
                danceCompHelperDb,
                newDbFile);

            if (File.Exists(
                newDbFile))
            {
                File.Delete(
                    newDbFile);
            }

            var newDb = new DanceCompetitionHelperDbContext(
                newDbFile);
            newDb.Database.Migrate();

            var newDbPoco = new DanceCompetitionHelperDbContextPoco(
                newDb,
                newDbFile);
            _scenarioContext.AddToScenarioContext(
                SpecFlowConstants.DanceCompetitionHelperDb,
                danceCompHelperDb,
                newDbPoco);

            AddToDispose(
                newDbPoco);
        }

        [Given(@"following Competitions in ""([^""]*)""")]
        public void GivenFollowingCompetitionsIn(
            string danceCompHelperDb,
            Table table)
        {
            var newComps = table.CreateSet<CompetitionPoco>();
            var useDb = GetDanceCompetitionHelperDbContext(
                danceCompHelperDb);

            using (var dbTrans = useDb.Database.BeginTransaction())
            {
                foreach (var newComp in newComps)
                {
                    try
                    {
                        useDb.Competitions.Add(
                            new Tables.Competition()
                            {
                                Organization = newComp.Organization,
                                OrgCompetitionId = newComp.OrgCompetitionId
                                    ?? throw new ArgumentNullException(
                                        nameof(newComp.OrgCompetitionId)),
                                CompetitionName = newComp.CompetitionName
                                    ?? throw new ArgumentNullException(
                                        nameof(newComp.CompetitionName)),
                                CompetitionInfo = newComp.CompetitionInfo,
                                CompetitionDate = newComp.CompetitionDate ?? UseNow,
                            });

                        useDb.SaveChanges();
                    }
                    catch (Exception exc)
                    {
                        dbTrans.Rollback();

                        Console.WriteLine(
                            "Error during add of '{0}': {1}",
                            newComp,
                            exc);
                        throw;
                    }
                }

                dbTrans.Commit();
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            DisposeAllCreated();
        }
    }
}
