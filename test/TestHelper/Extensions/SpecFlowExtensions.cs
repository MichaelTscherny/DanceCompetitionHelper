using Reqnroll;

namespace TestHelper.Extensions
{
    public static class SpecFlowExtensions
    {
        public static Dictionary<string, T>? GetContextPart<T>(
            this ScenarioContext scenarioContext,
            string contextPart)
            where T : class
        {
            if (scenarioContext == null)
            {
                return null;
            }

            Dictionary<string, T> contextItems;
            if (scenarioContext.TryGetValue(
                contextPart,
                out contextItems) == false)
            {
                contextItems = new Dictionary<string, T>();
                scenarioContext[contextPart] = contextItems;
            }

            return contextItems;
        }

        public static void AddToScenarioContext<T>(
            this ScenarioContext scenarioContext,
            string contextPart,
            string itemName,
            T item)
            where T : class
        {
            var contextItems = scenarioContext.GetContextPart<T>(
                contextPart);

            if (contextItems == null)
            {
                throw new ArgumentNullException(
                    nameof(contextPart));
            }

            contextItems.Add(
                itemName,
                item);
        }

        public static T? GetFromScenarioContext<T>(
            this ScenarioContext scenarioContext,
            string contextPart,
            string itemName)
            where T : class
        {
            var contextItems = scenarioContext.GetContextPart<T>(
                contextPart);

            if (contextItems == null)
            {
                return default(T);
            }

            if (contextItems.TryGetValue(
                itemName,
                out var toRet) == false)
            {
                return default(T);
            }

            return toRet;
        }
    }
}
