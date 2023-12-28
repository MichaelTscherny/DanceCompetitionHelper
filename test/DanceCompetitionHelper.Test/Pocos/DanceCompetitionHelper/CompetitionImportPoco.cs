using DanceCompetitionHelper.Database.Enum;

namespace DanceCompetitionHelper.Test.Pocos.DanceCompetitionHelper
{
    public class CompetitionImportPoco
    {
        public OrganizationEnum Organization { get; set; }
        public string? OrgCompetitionId { get; set; }

        public string? CompetitionFile { get; set; }
        public string? CompetitionClassesFile { get; set; }
        public string? ParticipantsFile { get; set; }

        public bool FindFollowUpClasses { get; set; }
    }
}
