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
            bool includeInfos = false);
        IAsyncEnumerable<CompetitionClass> GetCompetitionClassesAsync(
            Guid? competitionId,
            CancellationToken cancellationToken,
            bool includeInfos = false,
            bool showAll = false);
        IAsyncEnumerable<CompetitionVenue> GetCompetitionVenuesAsync(
            Guid? competitionId,
            CancellationToken cancellationToken);
        IAsyncEnumerable<Participant> GetParticipantsAsync(
            Guid? competitionId,
            Guid? competitionClassId,
            CancellationToken cancellationToken,
            bool includeInfos = false,
            bool showAll = false,
            ParticipantFilter? filter = null);
        IAsyncEnumerable<AdjudicatorPanel> GetAdjudicatorPanelsAsync(
            Guid? competitionId,
            CancellationToken cancellationToken,
            bool includeInfos = false);
        IAsyncEnumerable<Adjudicator> GetAdjudicatorsAsync(
            Guid? competitionId,
            Guid? adjudicatorPanelId,
            CancellationToken cancellationToken,
            bool includeInfos = false);

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
            bool includeCompetition = false);
        Task<Participant?> GetParticipantAsync(
            Guid participantId,
            CancellationToken cancellationToken,
            bool includeCompetition = false,
            bool includeCompetitionClass = false);

        Task<AdjudicatorPanel?> GetAdjudicatorPanelAsync(
            Guid adjudicatorPanelId,
            CancellationToken cancellationToken,
            bool includeCompetition = false);
        Task<Adjudicator?> GetAdjudicatorAsync(
            Guid adjudicatorId,
            CancellationToken cancellationToken);

        IAsyncEnumerable<MultipleStarter> GetMultipleStarterAsync(
            Guid competitionId,
            CancellationToken cancellationToken);

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

        Task RemoveCompetitionClassAsync(
            CompetitionClass removeCompetitionClass,
            CancellationToken cancellationToken);

        #endregion CompetitionClass Crud 

        #region CompetitionVanue Crud

        Task CreateCompetitionVenueAsync(
            CompetitionVenue newCompetitionVenue,
            CancellationToken cancellationToken);

        Task RemoveCompetitionVenueAsync(
            CompetitionVenue removeCompetitionVenue,
            CancellationToken cancellationToken);

        #endregion CompetitionVanue Crud

        #region Participant Crud

        Task CreateParticipantAsync(
            Participant createParticipant,
            CancellationToken cancellationToken);

        Task RemoveParticipantAsync(
            Participant removeParticipant,
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
                CancellationToken cancellationToken);

        Task<ConfigurationValue?> GetConfigurationAsync(
            string key,
            CancellationToken cancellationToken);

        Task<ConfigurationValue?> GetConfigurationAsync(
            string key,
            OrganizationEnum organization,
            CancellationToken cancellationToken);

        Task<ConfigurationValue?> GetConfigurationAsync(
            string key,
            Competition? competition,
            CancellationToken cancellationToken);

        Task<ConfigurationValue?> GetConfigurationAsync(
            string key,
            CompetitionClass? competitionClass,
            CancellationToken cancellationToken);

        [Obsolete("really needed except from tests?..")]
        // TODO: really needed except from tests?..
        Task<ConfigurationValue?> GetConfigurationAsync(
            string key,
            Competition? competition,
            CompetitionVenue? competitionVenue,
            CancellationToken cancellationToken);

        Task<ConfigurationValue?> GetConfigurationAsync(
            string key,
            CompetitionClass? competitionClass,
            CompetitionVenue? competitionVenue,
            CancellationToken cancellationToken);

        Task<ConfigurationValue?> GetConfigurationAsync(
            ConfigurationValue? cfgValue,
            CancellationToken cancellationToken);

        Task CreateConfigurationAsync(
            ConfigurationValue crateConfigurationValue,
            CancellationToken cancellationToken);

        Task RemoveConfigurationAsync(
            ConfigurationValue removeConfigurationValue,
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
