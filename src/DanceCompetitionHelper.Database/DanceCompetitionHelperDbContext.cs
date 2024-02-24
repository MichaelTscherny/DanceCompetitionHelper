using DanceCompetitionHelper.Database.Config;
using DanceCompetitionHelper.Database.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace DanceCompetitionHelper.Database
{
    public class DanceCompetitionHelperDbContext : DbContext
    {
        private readonly ILogger<DanceCompetitionHelperDbContext> _logger;

        public IDbConfig SqLiteDbConfig { get; }
        public static string User { get; set; } = Environment.UserName;

        public virtual DbSet<Competition> Competitions { get; set; }
        public virtual DbSet<CompetitionClass> CompetitionClasses { get; set; }
        public virtual DbSet<CompetitionClassHistory> CompetitionClassesHistory { get; set; }
        public virtual DbSet<Participant> Participants { get; set; }
        public virtual DbSet<ParticipantHistory> ParticipantsHistory { get; set; }

        public virtual DbSet<AdjudicatorPanel> AdjudicatorPanels { get; set; }
        public virtual DbSet<AdjudicatorPanelHistory> AdjudicatorPanelsHistroy { get; set; }
        public virtual DbSet<Adjudicator> Adjudicators { get; set; }
        public virtual DbSet<AdjudicatorHistory> AdjudicatorsHistory { get; set; }

        public virtual DbSet<TableVersionInfo> TableVersionInfos { get; set; }
        public virtual DbSet<ConfigurationValue> Configurations { get; set; }

        public virtual DbSet<CompetitionVenue> CompetitionVenues { get; set; }

        public DanceCompetitionHelperDbContext(
            IDbConfig sqLiteDbConfig,
            ILogger<DanceCompetitionHelperDbContext> logger)
            : base()
        {
            SqLiteDbConfig = sqLiteDbConfig;
            _logger = logger
                ?? throw new ArgumentNullException(
                    nameof(logger));

            if (SqLiteDbConfig == null
                || string.IsNullOrEmpty(SqLiteDbConfig.SqLiteDbFile)
                || string.IsNullOrWhiteSpace(SqLiteDbConfig.SqLiteDbFile))
            {
                throw new ArgumentNullException(
                    nameof(sqLiteDbConfig));
            }

            SavingChanges += OnSavingChanges;

            var testSqlFile = new FileInfo(SqLiteDbConfig.SqLiteDbFile);
            if (testSqlFile.Exists == false)
            {
                _logger.LogDebug(
                    "Check/Create DB file-directory '{SqLiteDbFileDirectory}'",
                    testSqlFile.Directory?.FullName);

                testSqlFile.Directory?.Create();
            }

            _logger.LogTrace(
                "{Method}() done",
                nameof(DanceCompetitionHelperDbContext));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .EnableSensitiveDataLogging()
                // .EnableDetailedErrors()
                /*
                .ConfigureWarnings(
                    cfg => cfg.Log(
                        (RelationalEventId.MigrationApplying, LogLevel.Information),
                        (RelationalEventId.CommandInitialized, LogLevel.Information),
                        (RelationalEventId.CommandExecuted, LogLevel.Information)))
                */
                .UseSqlite(
                    string.Format(
                        "Data Source='{0}'",
                        SqLiteDbConfig.SqLiteDbFile));

            _logger.LogTrace(
                "{Method}() done",
                nameof(OnConfiguring));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            _logger.LogTrace(
                "{Method}() done",
                nameof(OnModelCreating));

        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            /*
            configurationBuilder
                .Properties<DateTime>()
                .
            */

            base.ConfigureConventions(configurationBuilder);

            _logger.LogTrace(
                "{Method}() done",
                nameof(ConfigureConventions));
        }

        #region Usefull methods

        public void Migrate()
            => Database.Migrate();

        public IDbContextTransaction? BeginTransaction(
            bool useTransaction = true)
            => useTransaction
                ? Database.CurrentTransaction ?? Database.BeginTransaction()
                : null;

        #endregion Usefull methods

        #region Helpers

        private void OnSavingChanges(
            object? sender,
            SavingChangesEventArgs e)
        {
            var useDateTime = DateTime.UtcNow;

            foreach (var curEntity in ChangeTracker.Entries())
            {
                UpdateTimestamps(
                    curEntity,
                    useDateTime);
            }

            _logger.LogTrace(
                "{Method}() done",
                nameof(OnSavingChanges));
        }

        private static void UpdateTimestamps(
            EntityEntry entity,
            DateTime useDateTime)
        {
            if (entity == null
                || entity.Entity == null)
            {
                return;
            }

            if (entity.Entity is TableBase entityWithTimestamps)
            {
                switch (entity.State)
                {
                    case EntityState.Added:
                        entityWithTimestamps.Created = useDateTime;
                        entityWithTimestamps.CreatedBy = User;

                        entityWithTimestamps.LastModified = useDateTime;
                        entityWithTimestamps.LastModifiedBy = User;
                        break;

                    case EntityState.Modified:
                        entityWithTimestamps.LastModified = useDateTime;
                        entityWithTimestamps.LastModifiedBy = User;
                        break;
                }
            }
        }

        #endregion // Helpers
    }
}