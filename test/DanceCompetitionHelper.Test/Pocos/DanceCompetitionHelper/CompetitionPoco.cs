using DanceCompetitionHelper.Database.Enum;

namespace DanceCompetitionHelper.Database.Test.Pocos.DanceCompetitionHelper
{
    internal class CompetitionPoco
    {
        public string CompetitionName { get; set; } = default!;
        public OrganizationEnum Organization { get; set; }
        public string? OrgCompetitionId { get; set; }
        public string? CompetitionInfo { get; set; }
        public DateTime? CompetitionDate { get; set; }

        public CompetitionPoco ValidateCreate()
        {
            if (string.IsNullOrEmpty(
                OrgCompetitionId))
            {
                throw new ArgumentNullException(
                    nameof(OrgCompetitionId));
            }

            if (string.IsNullOrEmpty(
                CompetitionName))
            {
                throw new ArgumentNullException(
                    nameof(CompetitionName));
            }

            return this;
        }

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
