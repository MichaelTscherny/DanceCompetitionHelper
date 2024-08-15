﻿using DanceCompetitionHelper.Data;
using DanceCompetitionHelper.Database;
using DanceCompetitionHelper.Database.DisplayInfo;
using DanceCompetitionHelper.Database.Enum;
using DanceCompetitionHelper.Database.Tables;
using Microsoft.EntityFrameworkCore.Storage;
using System.Runtime.CompilerServices;

namespace DanceCompetitionHelper
{
    public interface IDanceCompetitionHelper : IDisposable
    {
        #region Administration stuff

        DanceCompetitionHelperDbContext GetDbCtx();
        Task AddTestDataAsync(
            CancellationToken cancellationToken);
        Task MigrateAsync();
        Task CheckMandatoryConfigurationAsync(
            CancellationToken cancellationToken);
        Task CreateTableHistoryAsync(
            Guid competitionId,
            CancellationToken cancellationToken,
            string comment = "Generated by User");

        #endregion Administration stuff

        IAsyncEnumerable<Competition> GetCompetitionsAsync(
            CancellationToken cancellationToken,
            bool includeInfos = false,
            bool useTransaction = true);
        IAsyncEnumerable<CompetitionClass> GetCompetitionClassesAsync(
            Guid? competitionId,
            CancellationToken cancellationToken,
            bool includeInfos = false,
            bool showAll = false,
            bool useTransaction = true);
        IAsyncEnumerable<CompetitionVenue> GetCompetitionVenuesAsync(
            Guid? competitionId,
            CancellationToken cancellationToken,
            bool useTransaction = true);
        IAsyncEnumerable<Participant> GetParticipantsAsync(
            Guid? competitionId,
            Guid? competitionClassId,
            CancellationToken cancellationToken,
            bool includeInfos = false,
            bool showAll = false,
            bool useTransaction = true);
        IAsyncEnumerable<AdjudicatorPanel> GetAdjudicatorPanelsAsync(
            Guid? competitionId,
            CancellationToken cancellationToken,
            bool includeInfos = false,
            bool useTransaction = true);
        IAsyncEnumerable<Adjudicator> GetAdjudicatorsAsync(
            Guid? competitionId,
            Guid? adjudicatorPanelId,
            CancellationToken cancellationToken,
            bool includeInfos = false,
            bool useTransaction = true);

        Task<Competition?> GetCompetitionAsync(
            Guid? competitionId,
            CancellationToken cancellationToken);

        Task<Competition?> FindCompetitionAsync(
            Guid? byAnyId,
            CancellationToken cancellationToken);
        Task<CompetitionClass?> FindCompetitionClassAsync(
            Guid? byAnyId,
            CancellationToken cancellationToken);
        Task<Competition?> GetCompetitionAsync(
            string byName,
            CancellationToken cancellationToken);
        Task<CompetitionClass?> GetCompetitionClassAsync(
            string byName,
            CancellationToken cancellationToken);
        Task<CompetitionClass?> GetCompetitionClassAsync(
            Guid competitionClassId,
            CancellationToken cancellationToken);
        Task<CompetitionVenue?> GetCompetitionVenueAsync(
            Guid? competitionVenueId,
            CancellationToken cancellationToken,
            bool useTransaction = true);
        Task<Participant?> GetParticipantAsync(
            Guid participantId,
            CancellationToken cancellationToken,
            bool useTransaction = true);

        Task<AdjudicatorPanel?> GetAdjudicatorPanelAsync(
            Guid adjudicatorPanelId,
            CancellationToken cancellationToken,
            bool includeCompetition = false);
        Task<Adjudicator?> GetAdjudicatorAsync(
            Guid adjudicatorId,
            CancellationToken cancellationToken);

        IAsyncEnumerable<MultipleStarter> GetMultipleStarterAsync(
            Guid competitionId,
            CancellationToken cancellationToken,
            bool useTransaction = true);

        #region Competition Crud

        Task CreateCompetitionAsync(
            Competition newCompetition,
            CancellationToken cancellationToken);

        Task RemoveCompetitionAsync(
            Competition removeCompetition,
            CancellationToken cancellationToken);

        #endregion Competition Crud

        #region AdjudicatorPanel Crud

        Task CreateAdjudicatorPanelAsync(
            AdjudicatorPanel newAdjudicatorPanel,
            CancellationToken cancellationToken);

        Task RemoveAdjudicatorPanelAsync(
            AdjudicatorPanel removeAdjudicatorPanel,
            CancellationToken cancellationToken);

        #endregion AdjudicatorPanel Crud

        #region Adjudicator Crud

        Task CreateAdjudicatorAsync(
            Adjudicator newAdjudicator,
            CancellationToken cancellationToken);

        Task RemoveAdjudicatorAsync(
            Adjudicator removeAdjudicator,
            CancellationToken cancellationToken);

        #endregion Adjudicator Crud

        #region CompetitionClass Crud

        Task CreateCompetitionClassAsync(
            CompetitionClass createCompetitionClass,
            CancellationToken cancellationToken);

        Task EditCompetitionClassAsync(
            Guid competitionClassId,
            string competitionClassName,
            Guid? followUpCompetitionClassId,
            Guid adjudicatorPanelId,
            string orgClassId,
            string? discipline,
            string? ageClass,
            string? ageGroup,
            string? className,
            int minStartsForPromotion,
            double minPointsForPromotion,
            double pointsForFirst,
            int extraManualStarter,
            string? comment,
            bool ignore,
            string? competitionColor,
            CancellationToken cancellationToken);

        Task RemoveCompetitionClassAsync(
            CompetitionClass removeCompetitionClass,
            CancellationToken cancellationToken);

        #endregion CompetitionClass Crud 

        #region CompetitionVanue Crud

        Task CreateCompetitionVenueAsync(
            Guid competitionId,
            string name,
            int lengthInMeter,
            int widthInMeter,
            string? comment,
            CancellationToken cancellationToken);

        Task EditCompetitionVenueAsync(
            Guid competitionVenueId,
            string name,
            int lengthInMeter,
            int widthInMeter,
            string? comment,
            CancellationToken cancellationToken);

        Task RemoveCompetitionVenueAsync(
            Guid competitionVenueId,
            CancellationToken cancellationToken);

        #endregion CompetitionVanue Crud

        #region Participant Crud

        Task CreateParticipantAsync(
            Guid competitionId,
            Guid competitionClassId,
            int startNumber,
            string namePartA,
            string? orgIdPartA,
            string? namePartB,
            string? orgIdPartB,
            string? clubName,
            string? orgIdClub,
            double orgPointsPartA,
            int orgStartsPartA,
            int? minStartsForPromotionPartA,
            bool? orgAlreadyPromotedPartA,
            string? OrgAlreadyPromotedInfoPartA,
            double? orgPointsPartB,
            int? orgStartsPartB,
            int? minStartsForPromotionPartB,
            bool? orgAlreadyPromotedPartB,
            string? OrgAlreadyPromotedInfoPartB,
            string? comment,
            bool ignore,
            CancellationToken cancellationToken);

        Task EditParticipantAsync(
            Guid participantId,
            Guid competitionClassId,
            int startNumber,
            string namePartA,
            string? orgIdPartA,
            string? namePartB,
            string? orgIdPartB,
            string? clubName,
            string? orgIdClub,
            double orgPointsPartA,
            int orgStartsPartA,
            int? minStartsForPromotionPartA,
            bool? orgAlreadyPromotedPartA,
            string? OrgAlreadyPromotedInfoPartA,
            double? orgPointsPartB,
            int? orgStartsPartB,
            int? minStartsForPromotionPartB,
            bool? orgAlreadyPromotedPartB,
            string? OrgAlreadyPromotedInfoPartB,
            string? comment,
            bool ignore,
            CancellationToken cancellationToken);

        Task RemoveParticipantAsync(
            Guid participantId,
            CancellationToken cancellationToken);

        #endregion CompetitionClass Crud

        #region Configuration

        Task<(IEnumerable<ConfigurationValue>? ConfigurationValues,
            Competition? Competition,
            IEnumerable<Competition>? Competitions,
            IEnumerable<CompetitionClass>? CompetitionClasses,
            IEnumerable<CompetitionVenue>? CompetitionVenues)>
            GetConfigurationsAsync(
                Guid? competitionId,
                CancellationToken cancellationToken,
                bool useTransaction = true);

        Task<ConfigurationValue?> GetConfigurationAsync(
            string key,
            CancellationToken cancellationToken,
            bool useTransaction = true);

        Task<ConfigurationValue?> GetConfigurationAsync(
            string key,
            OrganizationEnum organization,
            CancellationToken cancellationToken,
            bool useTransaction = true);

        Task<ConfigurationValue?> GetConfigurationAsync(
            string key,
            Competition? competition,
            CancellationToken cancellationToken,
            bool useTransaction = true);

        Task<ConfigurationValue?> GetConfigurationAsync(
            string key,
            CompetitionClass? competitionClass,
            CancellationToken cancellationToken,
            bool useTransaction = true);

        [Obsolete("really needed except from tests?..")]
        // TODO: really needed except from tests?..
        Task<ConfigurationValue?> GetConfigurationAsync(
            string key,
            Competition? competition,
            CompetitionVenue? competitionVenue,
            CancellationToken cancellationToken,
            bool useTransaction = true);

        Task<ConfigurationValue?> GetConfigurationAsync(
            string key,
            CompetitionClass? competitionClass,
            CompetitionVenue? competitionVenue,
            CancellationToken cancellationToken,
            bool useTransaction = true);

        Task<ConfigurationValue?> GetConfigurationAsync(
            ConfigurationValue? cfgValue,
            CancellationToken cancellationToken,
            bool useTransaction = true);

        Task CreateConfigurationAsync(
            OrganizationEnum? organization,
            Guid? competitionId,
            Guid? competitionClassId,
            Guid? competitionVenueId,
            string key,
            string? value,
            string? comment,
            CancellationToken cancellationToken);

        Task EditConfigurationAsync(
            OrganizationEnum? organization,
            Guid? competitionId,
            Guid? competitionClassId,
            Guid? competitionVenueId,
            string key,
            string? value,
            string? comment,
            CancellationToken cancellationToken);

        Task RemoveConfigurationAsync(
            OrganizationEnum? organization,
            Guid? competitionId,
            Guid? competitionClassId,
            Guid? competitionVenueId,
            string key,
            CancellationToken cancellationToken);

        #endregion Configuration

        #region Importer

        Task<ImportOrUpdateCompetitionStatus> ImportOrUpdateCompetitionAsync(
            OrganizationEnum Organization,
            string? OrgCompetitionId,
            ImportTypeEnum ImportType,
            CancellationToken cancellationToken,
            IEnumerable<string>? filePaths,
            Dictionary<string, string>? parameters = null);

        #endregion Importer

        #region Transaction Helper

        Task<TModel?> RunInReadonlyTransaction<TModel>(
            Func<IDanceCompetitionHelper, DanceCompetitionHelperDbContext, IDbContextTransaction, CancellationToken, Task<TModel>> func,
            CancellationToken cancellationToken = default,
            bool rethrowException = true,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        Task<TModel?> RunInTransactionWithSaveChangesAndCommit<TModel>(
            Func<IDanceCompetitionHelper, DanceCompetitionHelperDbContext, IDbContextTransaction, CancellationToken, Task<object?>> func,
            Func<object?, CancellationToken, Task<TModel>> onSuccess,
            Func<object?, CancellationToken, Task<TModel>>? onNoData,
            Func<Exception, object?, CancellationToken, Task<TModel>>? onException,
            CancellationToken cancellationToken = default,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        #endregion Transaction Helper

    }
}
