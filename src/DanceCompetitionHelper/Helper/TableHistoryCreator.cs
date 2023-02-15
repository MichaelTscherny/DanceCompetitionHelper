using DanceCompetitionHelper.Database;
using DanceCompetitionHelper.Database.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DanceCompetitionHelper.Helper
{
    public class TableHistoryCreator
    {
        private readonly ILogger<TableHistoryCreator> _logger;

        public DanceCompetitionHelperDbContext DbCtx { get; }
        public Dictionary<string, TableVersionInfo> TableVersions { get; } = new Dictionary<string, TableVersionInfo>();
        public Guid CompetitionId { get; }
        public string Comment { get; }

        public TableHistoryCreator(
            ILogger<TableHistoryCreator> logger,
            DanceCompetitionHelperDbContext dbCtx,
            Guid competitionId,
            string comment)
        {
            _logger = logger ?? throw new ArgumentNullException(
                nameof(logger));
            DbCtx = dbCtx ?? throw new ArgumentNullException(
                nameof(dbCtx));
            CompetitionId = competitionId;

            Comment = comment;
            if (string.IsNullOrEmpty(Comment)
                || string.IsNullOrWhiteSpace(Comment))
            {
                throw new ArgumentNullException(
                    nameof(comment));
            }
        }

        private TableVersionInfo GetTableVersion(
            string tableName)
        {
            if (TableVersions.TryGetValue(
                tableName,
                out var tableVersionInfo) == false)
            {
                tableVersionInfo = DbCtx.TableVersionInfos
                    .TagWith(
                        nameof(GetTableVersion))
                    .OrderByDescending(
                        x => x.CurrentVersion)
                    .FirstOrDefault(
                        x => x.CompetitionId == CompetitionId
                        && x.TableName == tableName);

                if (tableVersionInfo == null)
                {
                    tableVersionInfo = DbCtx.TableVersionInfos.Add(
                        new TableVersionInfo()
                        {
                            CompetitionId = CompetitionId,
                            TableName = tableName,
                            CurrentVersion = 1,
                            Comment = this.Comment,
                        }).Entity;
                }
                else
                {
                    tableVersionInfo = DbCtx.TableVersionInfos.Add(
                        new TableVersionInfo()
                        {
                            CompetitionId = CompetitionId,
                            TableName = tableName,
                            CurrentVersion = tableVersionInfo.CurrentVersion + 1,
                            Comment = this.Comment,
                        }).Entity;
                }

                TableVersions[tableName] = tableVersionInfo;
            }

            return tableVersionInfo;
        }

        public void CreateHistory()
        {
            var foundComp = DbCtx.Competitions
                .TagWith(
                    nameof(CreateHistory) + "[0]")
                .First(
                    x => x.CompetitionId == CompetitionId);

            var foundAdjPanHistory = GetTableVersion(
                nameof(DbCtx.AdjudicatorPanels));
            var foundAdjHistory = GetTableVersion(
                nameof(DbCtx.Adjudicators));

            var foundCompClassesVersion = GetTableVersion(
                nameof(DbCtx.CompetitionClasses));
            var foundPartVersion = GetTableVersion(
                nameof(DbCtx.Participants));

            foreach (var toBackup in DbCtx.AdjudicatorPanels
                .TagWith(
                    nameof(CreateHistory) + "[AdjudicatorPanels]")
                .Where(
                    x => x.CompetitionId == CompetitionId))
            {
                DbCtx.AdjudicatorPanelsHistroy.Add(
                    new AdjudicatorPanelHistory()
                    {
                        AdjudicatorPanelHistoryId = toBackup.AdjudicatorPanelId,
                        CompetitionId = toBackup.CompetitionId,
                        Version = foundAdjPanHistory.CurrentVersion,
                        Name = toBackup.Name,
                        Comment = toBackup.Comment,
                    });

                foreach (var toBackupSub in DbCtx.Adjudicators
                    .TagWith(
                        nameof(CreateHistory) + "[Adjudicator]")
                    .Where(
                        x => x.AdjudicatorPanelId == toBackup.AdjudicatorPanelId))
                {
                    DbCtx.AdjudicatorsHistory.Add(
                        new AdjudicatorHistory()
                        {
                            AdjudicatorHistoryId = toBackupSub.AdjudicatorId,
                            AdjudicatorPanelHistoryId = toBackupSub.AdjudicatorPanelId,
                            AdjudicatorPanelHistoryVersion = foundAdjPanHistory.CurrentVersion,
                            Version = foundAdjHistory.CurrentVersion,
                            Abbreviation = toBackupSub.Abbreviation,
                            Name = toBackupSub.Name,
                            Comment = toBackupSub.Comment,
                        });
                }
            }

            foreach (var toBackup in DbCtx.CompetitionClasses
                .TagWith(
                    nameof(CreateHistory) + "[CompetitionClasses]")
                .Where(
                    x => x.CompetitionId == CompetitionId))
            {
                DbCtx.CompetitionClassesHistory.Add(
                    new CompetitionClassHistory()
                    {
                        CompetitionClassHistoryId = toBackup.CompetitionClassId,
                        OrgClassId = toBackup.OrgClassId,
                        CompetitionId = toBackup.CompetitionId,
                        AdjudicatorPanelHistoryId = toBackup.AdjudicatorPanelId,
                        AdjudicatorPanelHistoryVersion = foundAdjPanHistory.CurrentVersion,
                        Version = foundCompClassesVersion.CurrentVersion,
                        CompetitionClassName = toBackup.CompetitionClassName,
                        Discipline = toBackup.Discipline,
                        AgeClass = toBackup.AgeClass,
                        AgeGroup = toBackup.AgeGroup,
                        Class = toBackup.Class,
                        MinStartsForPromotion = toBackup.MinStartsForPromotion,
                        MinPointsForPromotion = toBackup.MinPointsForPromotion,
                        PointsForFirst = toBackup.PointsForFirst,
                        ExtraManualStarter = toBackup.ExtraManualStarter,
                        Comment = toBackup.Comment,
                        Ignore = toBackup.Ignore,
                    });
            }

            foreach (var toBackup in DbCtx.Participants
                .TagWith(
                    nameof(CreateHistory) + "[Participants]")
                .Where(
                    x => x.CompetitionId == CompetitionId))
            {
                // foundPartVersion

                DbCtx.ParticipantsHistory.Add(
                    new ParticipantHistory()
                    {
                        ParticipantHistoryId = toBackup.ParticipantId,
                        CompetitionId = toBackup.CompetitionId,
                        CompetitionClassHistoryId = toBackup.CompetitionClassId,
                        CompetitionClassHistoryVersion = foundCompClassesVersion.CurrentVersion,
                        Version = foundPartVersion.CurrentVersion,
                        StartNumber = toBackup.StartNumber,
                        NamePartA = toBackup.NamePartA,
                        OrgIdPartA = toBackup.OrgIdPartA,
                        NamePartB = toBackup.NamePartB,
                        OrgIdPartB = toBackup.OrgIdPartB,
                        ClubName = toBackup.ClubName,
                        OrgIdClub = toBackup.OrgIdClub,
                        OrgPointsPartA = toBackup.OrgPointsPartA,
                        OrgStartsPartA = toBackup.OrgStartsPartA,
                        MinStartsForPromotionPartA = toBackup.MinStartsForPromotionPartA,
                        OrgPointsPartB = toBackup.OrgPointsPartB,
                        OrgStartsPartB = toBackup.OrgStartsPartB,
                        MinStartsForPromotionPartB = toBackup.MinStartsForPromotionPartB,
                        Comment = toBackup.Comment,
                        Ignore = toBackup.Ignore,
                    });
            }
        }
    }
}

