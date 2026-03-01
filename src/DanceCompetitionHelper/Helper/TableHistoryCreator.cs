using DanceCompetitionHelper.Database;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DanceCompetitionHelper.Helper
{
    public class TableHistoryCreator
    {
        private readonly ILogger<TableHistoryCreator> _logger;

        public DanceCompetitionHelperDbContext? DbCtx { get; private set; }
        public Guid CompetitionId { get; private set; }
        public string? Comment { get; private set; }

        public TableHistoryCreator(
            ILogger<TableHistoryCreator> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(
                nameof(logger));
        }

        private TableVersionInfo GetTableVersion()
        {
            ArgumentNullException.ThrowIfNull(DbCtx);

            var tableVersionInfo = DbCtx.TableVersionInfos
                .TagWith(
                    nameof(GetTableVersion))
                .OrderByDescending(
                    x => x.CurrentVersion)
                .FirstOrDefault(
                    x => x.CompetitionId == CompetitionId);

            if (tableVersionInfo == null)
            {
                tableVersionInfo = DbCtx.TableVersionInfos.Add(
                    new TableVersionInfo()
                    {
                        CompetitionId = CompetitionId,
                        CurrentVersion = 1,
                        Comment = this.Comment ?? string.Empty,
                    }).Entity;
            }
            else
            {
                tableVersionInfo = DbCtx.TableVersionInfos.Add(
                    new TableVersionInfo()
                    {
                        CompetitionId = CompetitionId,
                        CurrentVersion = tableVersionInfo.CurrentVersion + 1,
                        Comment = this.Comment ?? string.Empty,
                    }).Entity;
            }

            return tableVersionInfo;
        }

        public void CreateHistory(
            DanceCompetitionHelperDbContext dbCtx,
            Guid competitionId,
            string comment)
        {
            DbCtx = dbCtx ?? throw new ArgumentNullException(
                nameof(dbCtx));
            CompetitionId = competitionId;

            Comment = comment;

            ArgumentNullException.ThrowIfNullOrEmpty(comment);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(comment);

            var foundComp = DbCtx.Competitions
                .TagWith(
                    nameof(CreateHistory) + "[0]")
                .First(
                    x => x.CompetitionId == CompetitionId);

            var foundCompHistory = GetTableVersion();

            foreach (var toBackup in DbCtx.AdjudicatorPanels
                .TagWith(
                    nameof(CreateHistory) + "[AdjudicatorPanels]")
                .Where(
                    x => x.CompetitionId == CompetitionId))
            {
                DbCtx.AdjudicatorPanelsHistory.Add(
                    toBackup.Map(
                        foundCompHistory)!);

                foreach (var toBackupSub in DbCtx.Adjudicators
                    .TagWith(
                        nameof(CreateHistory) + "[Adjudicator]")
                    .Where(
                        x => x.AdjudicatorPanelId == toBackup.AdjudicatorPanelId))
                {
                    DbCtx.AdjudicatorsHistory.Add(
                        toBackupSub.Map(
                            foundCompHistory)!);
                }
            }

            foreach (var toBackup in DbCtx.CompetitionClasses
                .TagWith(
                    nameof(CreateHistory) + "[CompetitionClasses]")
                .Where(
                    x => x.CompetitionId == CompetitionId))
            {
                DbCtx.CompetitionClassesHistory.Add(
                    toBackup.Map(
                        foundCompHistory)!);
            }

            foreach (var toBackup in DbCtx.CompetitionVenues
                .TagWith(
                    nameof(CreateHistory) + "[CompetitionVenues]")
                .Where(
                    x => x.CompetitionId == CompetitionId))
            {
                DbCtx.CompetitionVenuesHistory.Add(
                    toBackup.Map(
                        foundCompHistory)!);
            }

            foreach (var toBackup in DbCtx.Participants
                .TagWith(
                    nameof(CreateHistory) + "[Participants]")
                .Where(
                    x => x.CompetitionId == CompetitionId))
            {
                DbCtx.ParticipantsHistory.Add(
                    toBackup.Map(
                        foundCompHistory)!);
            }

            foreach (var toBackup in DbCtx.Configurations
                .TagWith(
                    nameof(CreateHistory) + "[Configurations]")
                .Where(
                    x => x.CompetitionId == CompetitionId))
            {
                DbCtx.ConfigurationsHistory.Add(
                    toBackup.Map(
                        foundCompHistory)!);
            }
        }
    }
}

