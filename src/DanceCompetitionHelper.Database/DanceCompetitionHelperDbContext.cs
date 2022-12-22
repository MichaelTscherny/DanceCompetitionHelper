using DanceCompetitionHelper.Database.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DanceCompetitionHelper.Database
{
    public class DanceCompetitionHelperDbContext : DbContext
    {
        public string SqLiteDbFile { get; } = string.Empty;
        public static string User { get; set; } = Environment.UserName;

        public virtual DbSet<Competition> Competitions { get; set; }
        public virtual DbSet<CompetitionClass> CompetitionClasses { get; set; }
        public virtual DbSet<Participant> Participants { get; set; }

        public DanceCompetitionHelperDbContext()
            : base()
        {
            SavingChanges += OnSavingChanges;
        }

        public DanceCompetitionHelperDbContext(
            string sqLiteDbFile)
            : this()
        {
            SqLiteDbFile = sqLiteDbFile;

            if (string.IsNullOrEmpty(SqLiteDbFile)
                || string.IsNullOrWhiteSpace(SqLiteDbFile))
            {
                throw new ArgumentNullException(
                    nameof(sqLiteDbFile));
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlite(
                    string.Format(
                        "Data Source='{0}'",
                        SqLiteDbFile));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            /*
            configurationBuilder
                .Properties<DateTime>()
                .
            */

            base.ConfigureConventions(configurationBuilder);
        }

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