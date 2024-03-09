
namespace DanceCompetitionHelper.Database.Test.Pocos.DanceCompetitionHelper
{
    public class CompetitionClassPoco
    {
        public string CompetitionName { get; set; } = null!;
        public string OrgClassId { get; set; } = null!;
        public string CompetitionClassName { get; set; } = null!;
        public string? FollowUpCompetitionClassName { get; set; }
        public string AdjudicatorPanelName { get; set; } = null!;
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

        public override bool Equals(object? obj)
        {
            return obj is CompetitionClassPoco poco &&
                   CompetitionName == poco.CompetitionName &&
                   OrgClassId == poco.OrgClassId &&
                   CompetitionClassName == poco.CompetitionClassName &&
                   FollowUpCompetitionClassName == poco.FollowUpCompetitionClassName &&
                   AdjudicatorPanelName == poco.AdjudicatorPanelName &&
                   Discipline == poco.Discipline &&
                   AgeClass == poco.AgeClass &&
                   AgeGroup == poco.AgeGroup &&
                   Class == poco.Class &&
                   MinStartsForPromotion == poco.MinStartsForPromotion &&
                   MinPointsForPromotion == poco.MinPointsForPromotion &&
                   PointsForFirst == poco.PointsForFirst &&
                   CountParticipants == poco.CountParticipants &&
                   ExtraPartByWinning == poco.ExtraPartByWinning &&
                   ExtraPartByWinningInfo == poco.ExtraPartByWinningInfo &&
                   ExtraPartByPromotion == poco.ExtraPartByPromotion &&
                   ExtraPartByPromotionInfo == poco.ExtraPartByPromotionInfo &&
                   ExtraManualStarter == poco.ExtraManualStarter &&
                   Comment == poco.Comment &&
                   Ignore == poco.Ignore;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(CompetitionName);
            hash.Add(OrgClassId);
            hash.Add(CompetitionClassName);
            hash.Add(FollowUpCompetitionClassName);
            hash.Add(AdjudicatorPanelName);
            hash.Add(Discipline);
            hash.Add(AgeClass);
            hash.Add(AgeGroup);
            hash.Add(Class);
            hash.Add(MinStartsForPromotion);
            hash.Add(MinPointsForPromotion);
            hash.Add(PointsForFirst);
            hash.Add(CountParticipants);
            hash.Add(ExtraPartByWinning);
            hash.Add(ExtraPartByWinningInfo);
            hash.Add(ExtraPartByPromotion);
            hash.Add(ExtraPartByPromotionInfo);
            hash.Add(ExtraManualStarter);
            hash.Add(Comment);
            hash.Add(Ignore);
            return hash.ToHashCode();
        }

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
