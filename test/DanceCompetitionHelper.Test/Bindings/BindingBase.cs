using DanceCompetitionHelper.Database;
using DanceCompetitionHelper.Database.Test.Pocos;
using TestHelper.Extensions;

namespace DanceCompetitionHelper.Test.Bindings
{
    public abstract class BindingBase
    {
        public static readonly DateTime UseNow = DateTime.Now;

        protected readonly ScenarioContext _scenarioContext;

        private readonly List<IDisposable> _toDispose = new List<IDisposable>();

        public BindingBase(
            ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext
                ?? throw new ArgumentNullException(
                    nameof(scenarioContext));
        }

        public DanceCompetitionHelperDbContext GetDanceCompetitionHelperDbContext(
            string danceCompHelperDb)
        {
            var toRet = _scenarioContext.GetFromScenarioContext<DanceCompetitionHelperDbContextPoco>(
                SpecFlowConstants.DanceCompetitionHelperDb,
                danceCompHelperDb);

            if (toRet == null)
            {
                throw new ArgumentNullException(
                    nameof(danceCompHelperDb),
                    string.Format(
                        "{0} '{1}' not found!",
                        nameof(DanceCompetitionHelperDbContext),
                        danceCompHelperDb));
            }

            return toRet.DanceCompetitionHelperDbContext;
        }

        public IDanceCompetitionHelper GetDanceCompetitionHelper(
            string danceCompHelper)
        {
            var toRet = _scenarioContext.GetFromScenarioContext<IDanceCompetitionHelper>(
                SpecFlowConstants.DanceCompetitionHelper,
                danceCompHelper);

            if (toRet == null)
            {
                throw new ArgumentNullException(
                    nameof(danceCompHelper),
                    string.Format(
                        "{0} '{1}' not found!",
                        nameof(IDanceCompetitionHelper),
                        danceCompHelper));
            }

            return toRet;
        }

        #region Dispose Stuff

        public void AddToDispose(object toDispose)
        {
            if (toDispose is IDisposable toAdd)
            {
                _toDispose.Add(toAdd);
            }
        }

        public void DisposeAllCreated()
        {
            foreach (var toDispose in _toDispose)
            {
                var itemToDispose = toDispose?.GetType().FullName;

                try
                {
                    toDispose?.Dispose();

                    Console.WriteLine(
                        "disposed '{0}'",
                        itemToDispose);
                }
                catch (Exception exc)
                {
                    Console.WriteLine(
                        "Error during dispose of '{0}': {1}",
                        itemToDispose,
                        exc);
                }
            }
        }

        #endregion // Dispose Stuff
    }
}
