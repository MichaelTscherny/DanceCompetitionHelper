namespace DanceCompetitionHelper.Database.Tables
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;

    [Index(nameof(Created), IsUnique = false)]
    // TODO: [Index(nameof(LastModified), IsUnique = false)]
    public abstract class TableBase
    {
        [Required]
        [Comment("Row created at (UTC)")]
        public DateTime Created { get; set; }

        [Required]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthLastModified)]
        [Comment("Row created by")]
        public string CreatedBy { get; set; } = default!;

        [Required]
        [Comment("Row last modified at (UTC)")]
        public DateTime LastModified { get; set; }

        [Required]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthLastModified)]
        [Comment("Row last modified by")]
        public string LastModifiedBy { get; set; } = default!;
    }
}
