using DanceCompetitionHelper.Database;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Database.Test.Pocos;
using DanceCompetitionHelper.Database.Test.Pocos.DanceCompetitionHelper;
using Microsoft.EntityFrameworkCore;
using TestHelper.Extensions;

namespace DanceCompetitionHelper.Test.Bindings
{
    public abstract class BindingBase
    {
        public static readonly DateTime UseNow = DateTime.Now;

        protected readonly ScenarioContext _scenarioContext;

        private readonly List<IDisposable> _toDispose = new List<IDisposable>();

        private readonly Dictionary<DanceCompetitionHelperDbContext, Dictionary<string, Competition?>> _competitionsByName = new Dictionary<DanceCompetitionHelperDbContext, Dictionary<string, Competition?>>();
        private readonly Dictionary<DanceCompetitionHelperDbContext, Dictionary<Guid, Dictionary<string, AdjudicatorPanel?>>> _adjudicatorPanelsByCompIdAndName = new Dictionary<DanceCompetitionHelperDbContext, Dictionary<Guid, Dictionary<string, AdjudicatorPanel?>>>();
        private readonly Dictionary<DanceCompetitionHelperDbContext, Dictionary<Guid, Dictionary<string, CompetitionClass?>>> _competitionClassesByCompIdAndName = new Dictionary<DanceCompetitionHelperDbContext, Dictionary<Guid, Dictionary<string, CompetitionClass?>>>();
        private readonly Dictionary<DanceCompetitionHelperDbContext, Dictionary<Guid, Dictionary<string, CompetitionClassHistory?>>> _competitionClassesHistoryByCompIdAndName = new Dictionary<DanceCompetitionHelperDbContext, Dictionary<Guid, Dictionary<string, CompetitionClassHistory?>>>();

        private readonly Dictionary<DanceCompetitionHelperDbContext, Dictionary<Guid, Dictionary<string, CompetitionVenue?>>> _competitionVanesByCompIdAndName = new Dictionary<DanceCompetitionHelperDbContext, Dictionary<Guid, Dictionary<string, CompetitionVenue?>>>();

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

        #region Data Caching Stuff

        public Competition? GetCompetition(
            DanceCompetitionHelperDbContext dbCtx,
            string byCompetitionName)
        {
            if (dbCtx == null)
            {
                throw new ArgumentNullException(
                    nameof(dbCtx));
            }

            if (_competitionsByName.TryGetValue(
                dbCtx,
                out var byDbCtx) == false)
            {
                byDbCtx = new Dictionary<string, Competition?>();
                _competitionsByName[dbCtx] = byDbCtx;
            }

            if (byDbCtx.TryGetValue(
                byCompetitionName,
                out var foundComp) == false)
            {
                foundComp = dbCtx.Competitions
                    .TagWith(
                        nameof(GetCompetition))
                    .FirstOrDefault(
                        x => x.CompetitionName == byCompetitionName);

                byDbCtx[byCompetitionName] = foundComp;
            }

            return foundComp;
        }

        public AdjudicatorPanel? GetAdjudicatorPanel(
            DanceCompetitionHelperDbContext dbCtx,
            Guid byCompetitionId,
            string byAdjudicatorPanelName)
        {
            if (dbCtx == null)
            {
                throw new ArgumentNullException(
                    nameof(dbCtx));
            }

            if (_adjudicatorPanelsByCompIdAndName.TryGetValue(
                dbCtx,
                out var byDbCtx) == false)
            {
                byDbCtx = new Dictionary<Guid, Dictionary<string, AdjudicatorPanel?>>();
                _adjudicatorPanelsByCompIdAndName[dbCtx] = byDbCtx;
            }

            if (byDbCtx.TryGetValue(
                byCompetitionId,
                out var byDbCtxAndCompId) == false)
            {
                byDbCtxAndCompId = new Dictionary<string, AdjudicatorPanel?>();
                byDbCtx[byCompetitionId] = byDbCtxAndCompId;
            }


            if (byDbCtxAndCompId.TryGetValue(
                byAdjudicatorPanelName,
                out var foundAdjPanel) == false)
            {
                foundAdjPanel = dbCtx.AdjudicatorPanels
                    .TagWith(
                        nameof(GetAdjudicatorPanel))
                    .FirstOrDefault(
                        x => x.CompetitionId == byCompetitionId
                        && x.Name == byAdjudicatorPanelName);

                byDbCtxAndCompId[byAdjudicatorPanelName] = foundAdjPanel;
            }

            return foundAdjPanel;
        }

        public CompetitionClass? GetCompetitionClass(
            DanceCompetitionHelperDbContext dbCtx,
            Guid byCompetitionId,
            string? byCompetitionClassName)
        {
            if (dbCtx == null)
            {
                throw new ArgumentNullException(
                    nameof(dbCtx));
            }

            if (_competitionClassesByCompIdAndName.TryGetValue(
                dbCtx,
                out var byDbCtx) == false)
            {
                byDbCtx = new Dictionary<Guid, Dictionary<string, CompetitionClass?>>();
                _competitionClassesByCompIdAndName[dbCtx] = byDbCtx;
            }

            if (byDbCtx.TryGetValue(
                byCompetitionId,
                out var byDbCtxAndCompId) == false)
            {
                byDbCtxAndCompId = new Dictionary<string, CompetitionClass?>();
                byDbCtx[byCompetitionId] = byDbCtxAndCompId;
            }

            if (string.IsNullOrEmpty(
                byCompetitionClassName))
            {
                return null;
            }

            if (byDbCtxAndCompId.TryGetValue(
                byCompetitionClassName,
                out var foundCompClass) == false)
            {
                foundCompClass = dbCtx.CompetitionClasses
                    .TagWith(
                        nameof(GetCompetitionClass))
                    .FirstOrDefault(
                        x => x.CompetitionId == byCompetitionId
                        && x.CompetitionClassName == byCompetitionClassName);

                byDbCtxAndCompId[byCompetitionClassName] = foundCompClass;
            }

            return foundCompClass;
        }

        public CompetitionClassHistory? GetCompetitionClassHistory(
            DanceCompetitionHelperDbContext dbCtx,
            Guid byCompetitionId,
            string? byCompetitionClassName)
        {
            if (dbCtx == null)
            {
                throw new ArgumentNullException(
                    nameof(dbCtx));
            }

            if (_competitionClassesHistoryByCompIdAndName.TryGetValue(
                dbCtx,
                out var byDbCtx) == false)
            {
                byDbCtx = new Dictionary<Guid, Dictionary<string, CompetitionClassHistory?>>();
                _competitionClassesHistoryByCompIdAndName[dbCtx] = byDbCtx;
            }

            if (byDbCtx.TryGetValue(
                byCompetitionId,
                out var byDbCtxAndCompId) == false)
            {
                byDbCtxAndCompId = new Dictionary<string, CompetitionClassHistory?>();
                byDbCtx[byCompetitionId] = byDbCtxAndCompId;
            }

            if (string.IsNullOrEmpty(
                byCompetitionClassName))
            {
                return null;
            }

            if (byDbCtxAndCompId.TryGetValue(
                byCompetitionClassName,
                out var foundCompClass) == false)
            {
                foundCompClass = dbCtx.CompetitionClassesHistory
                    .TagWith(
                        nameof(GetCompetitionClass))
                    .FirstOrDefault(
                        x => x.CompetitionId == byCompetitionId
                        && x.CompetitionClassName == byCompetitionClassName);

                byDbCtxAndCompId[byCompetitionClassName] = foundCompClass;
            }

            return foundCompClass;
        }

        public CompetitionVenue? GetCompetitionVenue(
            DanceCompetitionHelperDbContext dbCtx,
            Guid byCompetitionId,
            string? byCompetitionVenueName)
        {
            if (dbCtx == null)
            {
                throw new ArgumentNullException(
                    nameof(dbCtx));
            }

            if (_competitionVanesByCompIdAndName.TryGetValue(
                dbCtx,
                out var byDbCtx) == false)
            {
                byDbCtx = new Dictionary<Guid, Dictionary<string, CompetitionVenue?>>();
                _competitionVanesByCompIdAndName[dbCtx] = byDbCtx;
            }

            if (byDbCtx.TryGetValue(
                byCompetitionId,
                out var byDbCtxAndCompId) == false)
            {
                byDbCtxAndCompId = new Dictionary<string, CompetitionVenue?>();
                byDbCtx[byCompetitionId] = byDbCtxAndCompId;
            }

            if (string.IsNullOrEmpty(
                byCompetitionVenueName))
            {
                return null;
            }

            if (byDbCtxAndCompId.TryGetValue(
                byCompetitionVenueName,
                out var foundCompVenue) == false)
            {
                foundCompVenue = dbCtx.CompetitionVenues
                    .TagWith(
                        nameof(GetCompetitionVenue))
                    .FirstOrDefault(
                        x => x.CompetitionId == byCompetitionId
                        && x.Name == byCompetitionVenueName);

                byDbCtxAndCompId[byCompetitionVenueName] = foundCompVenue;
            }

            return foundCompVenue;
        }

        #endregion Data Caching Stuff

        #region Special Sort Stuff

        public static IEnumerable<CompetitionClassPoco> SortForCreation(
            IEnumerable<CompetitionClassPoco>? compClasses)
        {
            var retItems = new List<CompetitionClassPoco>();
            var workItems = (compClasses ?? Enumerable.Empty<CompetitionClassPoco>()).ToList();

            // ------------------------
            // all with no "deps"
            var filteredItems = workItems.Where(
                x => string.IsNullOrEmpty(
                    x.FollowUpCompetitionClassName))
                .ToList();

            retItems.AddRange(
                filteredItems);
            workItems = workItems
                .Except(
                    filteredItems)
                .ToList();

            // ------------------------
            // all with deps...
            while (workItems.Count >= 1)
            {
                var allRetItems = retItems
                    .Select(
                        x => x.CompetitionClassName)
                    .ToHashSet();

                filteredItems = workItems
                    .Where(
                        x => allRetItems.Contains(
                            x.FollowUpCompetitionClassName ?? string.Empty))
                    .ToList();

                //
                if (filteredItems.Count <= 0)
                {
                    break;
                }

                retItems.AddRange(
                    filteredItems);
                workItems = workItems
                    .Except(
                        filteredItems)
                    .ToList();
            }

            // ------------------------
            // add what's left
            retItems.AddRange(
                workItems);

            return retItems;
        }

        #endregion Special Sort Stuff

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

        #endregion Dispose Stuff
    }
}
