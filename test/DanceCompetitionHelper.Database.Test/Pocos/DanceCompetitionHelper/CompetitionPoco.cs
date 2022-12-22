using DanceCompetitionHelper.Database.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanceCompetitionHelper.Database.Test.Pocos.DanceCompetitionHelper
{
    internal class CompetitionPoco
    {
        public OrganizationEnum Organization { get; set; }
        public string? OrgCompetitionId { get; set; }
        public string CompetitionName { get; set; } = default!;
        public string? CompetitionInfo { get; set; }
        public DateTime? CompetitionDate { get; set; }

        public override string ToString()
        {
            return string.Format(
                "{0} ('{1}'/'{2}' - '{3}')",
                CompetitionName,
                Organization,
                OrgCompetitionId,
                CompetitionInfo);
        }
    }
}
