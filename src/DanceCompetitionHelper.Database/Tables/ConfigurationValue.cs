using DanceCompetitionHelper.Database.Data;
using DanceCompetitionHelper.Database.Enum;
using DanceCompetitionHelper.Database.Extensions;
using DanceCompetitionHelper.Database.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanceCompetitionHelper.Database.Tables
{
    [Comment("Configurations")]
    [PrimaryKey(nameof(ConfigurationValueId))]
    [Index(nameof(Organization), nameof(CompetitionId), nameof(CompetitionClassId), nameof(CompetitionVenueId), nameof(Key), IsUnique = true)]
    [Index(nameof(Key), IsUnique = false)]
    public class ConfigurationValue : TableBase, IDefaultTrim
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ConfigurationValueId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public OrganizationEnum? Organization { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Ref to " + nameof(Tables.Competition))]
        public Guid? CompetitionId { get; set; }

        [ForeignKey(nameof(CompetitionId))]
        public Competition? Competition { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Ref to " + nameof(Tables.CompetitionClass))]
        public Guid? CompetitionClassId { get; set; }

        [ForeignKey(nameof(CompetitionClassId))]
        public CompetitionClass? CompetitionClass { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Ref to " + nameof(Tables.CompetitionVenue))]
        public Guid? CompetitionVenueId { get; set; }

        [ForeignKey(nameof(CompetitionVenueId))]
        public CompetitionVenue? CompetitionVenue { get; set; }

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
            get
            {
                if (Organization != null
                    && Organization != OrganizationEnum.Any
                    && CompetitionId != null
                    /* && CompetitionClassId != null */
                    && CompetitionVenueId != null)
                {
                    return ConfigurationScopeEnum.CompetitionVenue;
                }

                if (Organization != null
                    && Organization != OrganizationEnum.Any
                    && CompetitionId != null
                    && CompetitionClassId != null
                    && CompetitionVenueId == null)
                {
                    return ConfigurationScopeEnum.CompetitionClass;
                }

                if (Organization != null
                    && Organization != OrganizationEnum.Any
                    && CompetitionId != null
                    && CompetitionClassId == null
                    && CompetitionVenueId == null)
                {
                    return ConfigurationScopeEnum.Competition;
                }

                if (Organization != null
                    && Organization != OrganizationEnum.Any
                    && CompetitionId == null
                    && CompetitionClassId == null
                    && CompetitionVenueId == null)
                {
                    return ConfigurationScopeEnum.Organization;
                }

                return ConfigurationScopeEnum.Global;
            }
        }

        public void SanityCheck()
        {
            var chkOrganizationEmpty = Organization == null || Organization == OrganizationEnum.Any;
            var chkCompetitionIdEmpty = CompetitionId == null;
            var chkCompetitionClassIdEmpty = CompetitionClassId == null;
            var chkCompetitionVenueIdEmpty = CompetitionVenueId == null;
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
                /* || chkCompetitionClassIdmpty */))
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

            if (chkCompetitionClassIdEmpty == false
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

        public void DefaultTrim()
        {
            Key = Key.DefaultTrim();
        }

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
