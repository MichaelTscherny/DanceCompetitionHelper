using DanceCompetitionHelper.Database.Enum;

namespace DanceCompetitionHelper.Test.Pocos.DanceCompetitionHelper
{
    internal class ConfigurationValuePoco
    {
        public OrganizationEnum? Organization { get; set; }
        public string? CompetitionName { get; set; }
        public string? CompetitionClassName { get; set; }
        public string? CompetitionVenueName { get; set; }
        public string Key { get; set; } = default!;
        public string? Value { get; set; }

        public void SanityCheck()
        {
            var chkOrganizationEmpty = Organization == null || Organization == OrganizationEnum.Any;
            var chkCompetitionNameEmpty = string.IsNullOrEmpty(CompetitionName);
            var chkCompetitionClassNameEmpty = string.IsNullOrEmpty(CompetitionClassName);
            var chkCompetitionVenueNameEmpty = string.IsNullOrEmpty(CompetitionVenueName);
            var chkKeyEmpty = string.IsNullOrEmpty(Key);

            if (chkKeyEmpty)
            {
                throw new ArgumentNullException(
                    nameof(Key),
                    ToString());
            }

            if (chkCompetitionVenueNameEmpty == false
                && (chkOrganizationEmpty
                || chkCompetitionNameEmpty
                /* || chkCompetitionClassNameEmpty */))
            {
                throw new ArgumentNullException(
                    string.Join(
                        ", ",
                        new[] {
                            nameof(Organization),
                            nameof(CompetitionName),
                            nameof(CompetitionClassName),
                        }),
                    ToString());
            }

            if (chkCompetitionClassNameEmpty == false
                && (chkOrganizationEmpty
                || chkCompetitionNameEmpty))
            {
                throw new ArgumentNullException(
                    string.Join(
                        ", ",
                        new[] {
                            nameof(Organization),
                            nameof(CompetitionName),
                        }),
                    ToString());
            }

            if (chkCompetitionNameEmpty == false
                && (chkOrganizationEmpty))
            {
                throw new ArgumentNullException(
                    string.Join(
                        ", ",
                        new[] {
                            nameof(Organization),
                        }),
                    ToString());
            }
        }

        public override string ToString()
        {
            return string.Format(
                "'{0}'/'{1}'/'{2}'/'{3}'/'{4}': {5}",
                Organization,
                CompetitionName,
                CompetitionClassName,
                CompetitionVenueName,
                Key,
                Value);
        }
    }
}
