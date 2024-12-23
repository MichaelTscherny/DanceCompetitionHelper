﻿using DanceCompetitionHelper.Database.Enum;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DanceCompetitionHelper.Web.Models.CompetitionModels
{
    public class DoImportViewModel : ViewModelBase
    {
        public List<string> WorkInfo { get; set; } = new List<string>();

        [FromForm]
        [HiddenInput]
        public Guid? CompetitionId { get; set; }

        [Required]
        [FromForm]
        [HiddenInput]
        public ImportTypeEnum ImportType { get; set; }

        [Required]
        [FromForm]
        [HiddenInput]
        public OrganizationEnum Organization { get; set; }

        [Required]
        [FromForm]
        public string? OrgCompetitionId { get; set; }

        [FromForm]
        public bool FindFollowUpClasses { get; set; }

        [FromForm]
        public bool UpdateData { get; set; }
    }
}
