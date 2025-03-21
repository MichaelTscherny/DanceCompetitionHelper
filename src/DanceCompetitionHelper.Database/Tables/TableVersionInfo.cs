﻿using DanceCompetitionHelper.Database.Extensions;
using DanceCompetitionHelper.Database.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanceCompetitionHelper.Database.Tables
{
    [PrimaryKey(nameof(CompetitionId), nameof(TableName), nameof(CurrentVersion))]
    public class TableVersionInfo : TableBase, IDefaultTrim
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Ref to " + nameof(Tables.Competition))]
        public Guid CompetitionId { get; set; }

        [ForeignKey(nameof(CompetitionId))]
        public Competition Competition { get; set; } = default!;

        [Required]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsLarge)]
        public string TableName { get; set; } = default!;

        [Required]
        public int CurrentVersion { get; set; }

        [Required]
        [MaxLength(DanceCompetitionHelperConstants.MaxLengthStringsLarge)]
        public string Comment { get; set; } = default!;

        public void DefaultTrim()
        {
            TableName = TableName.DefaultTrim();
        }
    }
}
