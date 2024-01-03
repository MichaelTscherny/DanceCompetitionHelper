using DanceCompetitionHelper.Database.Data;
using DanceCompetitionHelper.Database.Enum;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanceCompetitionHelper.Database.Tables
{
    [Comment("Configurations")]
    [PrimaryKey(nameof(Organization), nameof(CompetitionId), nameof(CompetitionClassId), nameof(CompetitionVenueId), nameof(Key))]
    [Index(nameof(Key), IsUnique = false)]
    public class ConfigurationValue : TableBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public OrganizationEnum Organization { get; set; } = OrganizationEnum.Any;

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Ref to " + nameof(Tables.Competition))]
        public Guid CompetitionId { get; set; } = Guid.Empty;

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Ref to " + nameof(Tables.CompetitionClass))]
        public Guid CompetitionClassId { get; set; } = Guid.Empty;

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Ref to " + nameof(Tables.CompetitionVenue))]
        public Guid CompetitionVenueId { get; set; } = Guid.Empty;

        [Required]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsLarge)]
        [Comment("Key of the Configuration Value")]
        public string Key { get; set; } = default!;

        [Comment("Value itself")]
        public string? Value { get; set; }

        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsShort)]
        public string? Comment { get; set; }

        [NotMapped]
        public ConfigurationScopeEnum Scope
        {
            // TODO: add tests!..
            get
            {
                if (Organization != OrganizationEnum.Any
                    && CompetitionId != Guid.Empty
                    && CompetitionClassId != Guid.Empty
                    && CompetitionVenueId != Guid.Empty)
                {
                    return ConfigurationScopeEnum.CompetitionVenue;
                }

                if (Organization != OrganizationEnum.Any
                    && CompetitionId != Guid.Empty
                    && CompetitionClassId != Guid.Empty
                    && CompetitionVenueId == Guid.Empty)
                {
                    return ConfigurationScopeEnum.CompetitionClass;
                }

                if (Organization != OrganizationEnum.Any
                    && CompetitionId != Guid.Empty
                    && CompetitionClassId == Guid.Empty
                    && CompetitionVenueId == Guid.Empty)
                {
                    return ConfigurationScopeEnum.Competition;
                }

                if (Organization != OrganizationEnum.Any
                    && CompetitionId == Guid.Empty
                    && CompetitionClassId == Guid.Empty
                    && CompetitionVenueId == Guid.Empty)
                {
                    return ConfigurationScopeEnum.Organization;
                }

                return ConfigurationScopeEnum.Global;
            }
        }

        public void SanityCheck()
        {
            var chkOrganizationEmpty = Organization == OrganizationEnum.Any;
            var chkCompetitionIdEmpty = CompetitionId == Guid.Empty;
            var chkCompetitionClassIdmpty = CompetitionClassId == Guid.Empty;
            var chkCompetitionVenueIdEmpty = CompetitionVenueId == Guid.Empty;
            var chkKeyEmpty = string.IsNullOrEmpty(Key);

            if (chkKeyEmpty)
            {
                throw new ArgumentNullException(
                    nameof(Key),
                    ToString());
            }

            if (chkCompetitionVenueIdEmpty == false
                && (chkOrganizationEmpty
                || chkCompetitionIdEmpty
                || chkCompetitionClassIdmpty))
            {
                throw new ArgumentNullException(
                    string.Join(
                        ", ",
                        new[] {
                            nameof(Organization),
                            nameof(CompetitionId),
                            nameof(CompetitionClassId),
                        }),
                    ToString());
            }

            if (chkCompetitionClassIdmpty == false
                && (chkOrganizationEmpty
                || chkCompetitionIdEmpty))
            {
                throw new ArgumentNullException(
                    string.Join(
                        ", ",
                        new[] {
                            nameof(Organization),
                            nameof(CompetitionId),
                        }),
                    ToString());
            }

            if (chkCompetitionIdEmpty == false
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

        [NotMapped]
        public ConfigurationValueParser ValueParser =>
            new ConfigurationValueParser(this);

        public override string ToString()
        {
            return string.Format(
                "'{0}'[{1}] = '{2}'",
                Key,
                Scope,
                Value);
        }
    }
}
