﻿namespace DanceCompetitionHelper.Database.Test.Pocos.DanceCompetitionHelper
{
    public class CompetitionClassPoco
    {
        public string CompetitionName { get; set; } = default!;
        public string OrgClassId { get; set; } = default!;
        public string CompetitionClassName { get; set; } = default!;
        public string? FollowUpCompetitionClassName { get; set; }
        public string AdjudicatorPanelName { get; set; } = default!;
        public string? Discipline { get; set; }
        public string? AgeClass { get; set; }
        public string? AgeGroup { get; set; }
        public string? Class { get; set; }
        public int? MinStartsForPromotion { get; set; }
        public int? MinPointsForPromotion { get; set; }
        public int? PointsForFirst { get; set; }

        public int CountParticipants { get; set; }
        public int ExtraPartByWinning { get; set; }
        public string? ExtraPartByWinningInfo { get; set; }
        public int ExtraPartByPromotion { get; set; }
        public string? ExtraPartByPromotionInfo { get; set; }

        public int ExtraManualStarter { get; set; }

        public string? Comment { get; set; }

        public bool Ignore { get; set; }

        public override string ToString()
        {
            return string.Format(
                "{0} ('{1}'/'{2}'/'{3}') D/Ac/Ag/C: '{4}'/'{5}'/'{6}'/'{7}' - S/P: {8}/{9}",
                CompetitionName,
                CompetitionClassName,
                OrgClassId,
                "CURRENT",
                Discipline,
                AgeClass,
                AgeGroup,
                Class,
                MinStartsForPromotion,
                MinPointsForPromotion);
        }
    }
}
