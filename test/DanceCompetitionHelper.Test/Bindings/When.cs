using TechTalk.SpecFlow;

namespace DanceCompetitionHelper.Test.Bindings
{
    [Binding]
    public sealed class When : BindingBase
    {
        public When(
            ScenarioContext scenarioContext)
            : base(
                  scenarioContext)
        {
        }

        [AfterScenario]
        public void AfterScenario()
        {
            DisposeAllCreated();
        }
    }
}
