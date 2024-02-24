using DanceCompetitionHelper.Database.Enum;
using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Web.Helper.Configuration
{
    public class CompetitionVenueDisplayHelper
    {
        public Dictionary<string, Dictionary<OrganizationEnum, Dictionary<Guid, Dictionary<Guid, Dictionary<Guid, ConfigurationValue>>>>> AllConfigKeysBy { get; } = new Dictionary<string, Dictionary<OrganizationEnum, Dictionary<Guid, Dictionary<Guid, Dictionary<Guid, ConfigurationValue>>>>>();

        public IEnumerable<string> Keys => AllConfigKeysBy
            .Keys
            .OrderBy(
                x => x)
            .ToList();

        public CompetitionVenueDisplayHelper(
            IEnumerable<ConfigurationValue>? configurationValues)
        {
            AddValues(
                configurationValues);
        }

        public void AddValues(
            IEnumerable<ConfigurationValue>? configurationValues)
        {
            if (configurationValues == null)
            {
                return;
            }

            foreach (var curItem in configurationValues)
            {
                if (curItem == null)
                {
                    continue;
                }

                if (AllConfigKeysBy.TryGetValue(
                    curItem.Key,
                    out var foundByKey) == false)
                {
                    foundByKey = new Dictionary<OrganizationEnum, Dictionary<Guid, Dictionary<Guid, Dictionary<Guid, ConfigurationValue>>>>();
                    AllConfigKeysBy[curItem.Key] = foundByKey;
                }

                var useOrganization = curItem.Organization ?? OrganizationEnum.Any;
                if (foundByKey.TryGetValue(
                    useOrganization,
                    out var foundByKeyAndOrg) == false)
                {
                    foundByKeyAndOrg = new Dictionary<Guid, Dictionary<Guid, Dictionary<Guid, ConfigurationValue>>>();
                    foundByKey[useOrganization] = foundByKeyAndOrg;
                }

                var useCompetitionId = curItem.CompetitionId ?? Guid.Empty;
                if (foundByKeyAndOrg.TryGetValue(
                    useCompetitionId,
                    out var foundByKeyAndOrgAndCompId) == false)
                {
                    foundByKeyAndOrgAndCompId = new Dictionary<Guid, Dictionary<Guid, ConfigurationValue>>();
                    foundByKeyAndOrg[useCompetitionId] = foundByKeyAndOrgAndCompId;
                }

                var useCompetitionClassId = curItem.CompetitionClassId ?? Guid.Empty;
                if (foundByKeyAndOrgAndCompId.TryGetValue(
                    useCompetitionClassId,
                    out var foundByKeyAndOrgAndCompIdAndCompClassId) == false)
                {
                    foundByKeyAndOrgAndCompIdAndCompClassId = new Dictionary<Guid, ConfigurationValue>();
                    foundByKeyAndOrgAndCompId[useCompetitionClassId] = foundByKeyAndOrgAndCompIdAndCompClassId;
                }

                foundByKeyAndOrgAndCompIdAndCompClassId[curItem.CompetitionVenueId ?? Guid.Empty] = curItem;
            }
        }

        public ConfigurationValue? Get(
            string key)
        {
            return Get(
                key,
                OrganizationEnum.Any);
        }

        public ConfigurationValue? Get(
            string key,
            OrganizationEnum? organization)
        {
            return Get(
                key,
                organization,
                null);
        }

        public ConfigurationValue? Get(
            string key,
            OrganizationEnum? organization,
            Guid? competitionId)
        {
            return Get(
                key,
                organization,
                competitionId,
                null);
        }

        public ConfigurationValue? Get(
            string key,
            OrganizationEnum? organization,
            Guid? competitionId,
            Guid? competitionClassId)
        {
            return Get(
                key,
                organization,
                competitionId,
                competitionClassId,
                null);
        }

        public ConfigurationValue? Get(
            string key,
            OrganizationEnum? organization,
            Guid? competitionId,
            Guid? competitionClassId,
            Guid? competitionVenueId)
        {
            if (AllConfigKeysBy.TryGetValue(
                key,
                out var byKey) == false)
            {
                return null;
            }

            if (byKey.TryGetValue(
                organization ?? OrganizationEnum.Any,
                out var byKeyAndOrg) == false)
            {
                return null;
            }

            if (byKeyAndOrg.TryGetValue(
                competitionId ?? Guid.Empty,
                out var byKeyAndOrgAndCompId) == false)
            {
                return null;
            }

            if (byKeyAndOrgAndCompId.TryGetValue(
                competitionClassId ?? Guid.Empty,
                out var byKeyAndOrgAndCompIdAndCompClassId) == false)
            {
                return null;
            }

            byKeyAndOrgAndCompIdAndCompClassId.TryGetValue(
                competitionVenueId ?? Guid.Empty,
                out var foundConfigVal);

            return foundConfigVal;
        }
    }
}
