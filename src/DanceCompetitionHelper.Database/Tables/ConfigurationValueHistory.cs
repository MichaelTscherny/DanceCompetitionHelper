using DanceCompetitionHelper.Database.Enum;
using DanceCompetitionHelper.Database.Extensions;
using DanceCompetitionHelper.Database.Interfaces;

using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanceCompetitionHelper.Database.Tables
{
    [Comment("Configurations")]
    [PrimaryKey(nameof(ConfigurationValueHistoryId), nameof(Version))]
    [Index(nameof(Organization), nameof(CompetitionId), nameof(CompetitionClassHistroyId), nameof(CompetitionVenueHistoryId), nameof(Key), nameof(Version), IsUnique = true)]
    [Index(nameof(Key), IsUnique = false)]
    public class ConfigurationValueHistory : TableBase, IDefaultTrim
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ConfigurationValueHistoryId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public OrganizationEnum? Organization { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Ref to " + nameof(Tables.Competition))]
        public Guid? CompetitionId { get; set; }

        [ForeignKey(nameof(CompetitionId))]
        public Competition? Competition { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Ref to " + nameof(Tables.CompetitionClass))]
        public Guid? CompetitionClassHistroyId { get; set; }

        [ForeignKey(nameof(CompetitionClassHistroyId) + "," + nameof(Version))]
        public CompetitionClassHistory? CompetitionClassHistory { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Ref to " + nameof(Tables.CompetitionVenue))]
        public Guid? CompetitionVenueHistoryId { get; set; }

        [ForeignKey(nameof(CompetitionVenueHistoryId) + "," + nameof(Version))]
        public CompetitionVenueHistory? CompetitionVenueHistory { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Version { get; set; }

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
                    && CompetitionVenueHistoryId != null)
                {
                    return ConfigurationScopeEnum.CompetitionVenue;
                }

                if (Organization != null
                    && Organization != OrganizationEnum.Any
                    && CompetitionId != null
                    && CompetitionClassHistroyId != null
                    && CompetitionVenueHistoryId == null)
                {
                    return ConfigurationScopeEnum.CompetitionClass;
                }

                if (Organization != null
                    && Organization != OrganizationEnum.Any
                    && CompetitionId != null
                    && CompetitionClassHistroyId == null
                    && CompetitionVenueHistoryId == null)
                {
                    return ConfigurationScopeEnum.Competition;
                }

                if (Organization != null
                    && Organization != OrganizationEnum.Any
                    && CompetitionId == null
                    && CompetitionClassHistroyId == null
                    && CompetitionVenueHistoryId == null)
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
            var chkCompetitionClassIdEmpty = CompetitionClassHistroyId == null;
            var chkCompetitionVenueIdEmpty = CompetitionVenueHistoryId == null;
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
                            nameof(CompetitionClassHistroyId),
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

        /* TODO: what to do here?..
        [NotMapped]
        public ConfigurationValueParser ValueParser =>
            new ConfigurationValueParser(this);
        */

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
