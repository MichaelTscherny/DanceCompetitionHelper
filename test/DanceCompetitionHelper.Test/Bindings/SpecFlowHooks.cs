﻿using DanceCompetitionHelper.Database.Test.Tests.UnitTests;

namespace DanceCompetitionHelper.Test.Bindings
{
    [Binding]
    public sealed class SpecFlowHooks
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            // Service.Instance.ValueRetrievers.Register(new MyCustomValueRetriever());

            DanceCompetitionHelperDbContextTests.CleanUpOldDbs();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
        }

        [BeforeScenario("@tag1")]
        public void BeforeScenarioWithTag()
        {
            // Example of filtering hooks using tags. (in this case, this 'before scenario' hook will execute if the feature/scenario contains the tag '@tag1')
            // See https://docs.specflow.org/projects/specflow/en/latest/Bindings/Hooks.html?highlight=hooks#tag-scoping

            // implement logic that has to run before executing each scenario
        }

        [BeforeScenario(Order = 1)]
        public void FirstBeforeScenario()
        {
            // Example of ordering the execution of hooks
            // See https://docs.specflow.org/projects/specflow/en/latest/Bindings/Hooks.html?highlight=order#hook-execution-order

            // implement logic that has to run before executing each scenario
        }

        [AfterScenario]
        public void AfterScenario()
        {
            // implement logic that has to run after executing each scenario
        }
    }
}