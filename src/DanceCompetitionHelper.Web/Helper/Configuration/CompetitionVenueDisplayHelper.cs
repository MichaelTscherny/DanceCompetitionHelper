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

                if (foundByKey.TryGetValue(
                    curItem.Organization,
                    out var foundByKeyAndOrg) == false)
                {
                    foundByKeyAndOrg = new Dictionary<Guid, Dictionary<Guid, Dictionary<Guid, ConfigurationValue>>>();
                    foundByKey[curItem.Organization] = foundByKeyAndOrg;
                }

                if (foundByKeyAndOrg.TryGetValue(
                    curItem.CompetitionId,
                    out var foundByKeyAndOrgAndCompId) == false)
                {
                    foundByKeyAndOrgAndCompId = new Dictionary<Guid, Dictionary<Guid, ConfigurationValue>>();
                    foundByKeyAndOrg[curItem.CompetitionId] = foundByKeyAndOrgAndCompId;
                }

                if (foundByKeyAndOrgAndCompId.TryGetValue(
                    curItem.CompetitionClassId,
                    out var foundByKeyAndOrgAndCompIdAndCompClassId) == false)
                {
                    foundByKeyAndOrgAndCompIdAndCompClassId = new Dictionary<Guid, ConfigurationValue>();
                    foundByKeyAndOrgAndCompId[curItem.CompetitionClassId] = foundByKeyAndOrgAndCompIdAndCompClassId;
                }

                foundByKeyAndOrgAndCompIdAndCompClassId[curItem.CompetitionVenueId] = curItem;
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
            OrganizationEnum organization)
        {
            return Get(
                key,
                organization,
                Guid.Empty);
        }

        public ConfigurationValue? Get(
            string key,
            OrganizationEnum organization,
            Guid competitionId)
        {
            return Get(
                key,
                organization,
                competitionId,
                Guid.Empty);
        }

        public ConfigurationValue? Get(
            string key,
            OrganizationEnum organization,
            Guid competitionId,
            Guid competitionClassId)
        {
            return Get(
                key,
                organization,
                competitionId,
                competitionClassId,
                Guid.Empty);
        }

        public ConfigurationValue? Get(
            string key,
            OrganizationEnum organization,
            Guid competitionId,
            Guid competitionClassId,
            Guid competitionVenueId)
        {
            if (AllConfigKeysBy.TryGetValue(
                key,
                out var byKey) == false)
            {
                return null;
            }

            if (byKey.TryGetValue(
                organization,
                out var byKeyAndOrg) == false)
            {
                return null;
            }

            if (byKeyAndOrg.TryGetValue(
                competitionId,
                out var byKeyAndOrgAndCompId) == false)
            {
                return null;
            }

            if (byKeyAndOrgAndCompId.TryGetValue(
                competitionClassId,
                out var byKeyAndOrgAndCompIdAndCompClassId) == false)
            {
                return null;
            }

            byKeyAndOrgAndCompIdAndCompClassId.TryGetValue(
                competitionVenueId,
                out var foundConfigVal);

            return foundConfigVal;
        }
    }
}
