using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Web.Models.AdjudicatorModels;
using DanceCompetitionHelper.Web.Models.AdjudicatorPanelModels;
using DanceCompetitionHelper.Web.Models.CompetitionClassModels;
using DanceCompetitionHelper.Web.Models.CompetitionModels;
using DanceCompetitionHelper.Web.Models.CompetitionVenueModels;
using DanceCompetitionHelper.Web.Models.ConfigurationModels;
using DanceCompetitionHelper.Web.Models.ParticipantModels;

namespace DanceCompetitionHelper.Web.Extensions
{
    public static class TypeMapping
    {
        #region AdjudicatorPanel

        public static AdjudicatorPanelViewModel? Map(
            this AdjudicatorPanel source,
            AdjudicatorPanelViewModel? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new AdjudicatorPanelViewModel();
            ret.CompetitionId = source.CompetitionId;
            ret.AdjudicatorPanelId = source.AdjudicatorPanelId;
            ret.Name = source.Name;
            ret.Comment = source.Comment;

            return ret;
        }

        public static AdjudicatorPanel? Map(
            this AdjudicatorPanelViewModel source,
            AdjudicatorPanel? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new AdjudicatorPanel();
            ret.CompetitionId = source.CompetitionId;
            ret.AdjudicatorPanelId = source.AdjudicatorPanelId ?? Guid.Empty;
            ret.Name = source.Name;
            ret.Comment = source.Comment;

            return ret;
        }

        #endregion AdjudicatorPanel

        #region Adjudicator

        public static AdjudicatorViewModel? Map(
            this Adjudicator source,
            AdjudicatorViewModel? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new AdjudicatorViewModel();
            ret.AdjudicatorId = source.AdjudicatorId;
            ret.AdjudicatorPanelId = source.AdjudicatorPanelId;
            ret.Abbreviation = source.Abbreviation;
            ret.Name = source.Name;
            ret.Comment = source.Comment;

            return ret;
        }

        public static Adjudicator? Map(
            this AdjudicatorViewModel source,
            Adjudicator? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new Adjudicator();
            ret.AdjudicatorId = source.AdjudicatorId ?? Guid.Empty;
            ret.AdjudicatorPanelId = source.AdjudicatorPanelId;
            ret.Abbreviation = source.Abbreviation;
            ret.Name = source.Name;
            ret.Comment = source.Comment;

            return ret;
        }

        #endregion Adjudicator

        #region CompetitionClass

        public static CompetitionClassViewModel? Map(
            this CompetitionClass source,
            CompetitionClassViewModel? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new CompetitionClassViewModel();
            ret.CompetitionId = source.CompetitionId;
            ret.CompetitionClassId = source.CompetitionClassId;
            ret.CompetitionClassName = source.CompetitionClassName;
            ret.FollowUpCompetitionClassId = source.FollowUpCompetitionClassId;
            ret.AdjudicatorPanelId = source.AdjudicatorPanelId;
            ret.OrgClassId = source.OrgClassId;
            ret.Discipline = source.Discipline;
            ret.AgeClass = source.AgeClass;
            ret.AgeGroup = source.AgeGroup;
            ret.Class = source.Class;
            ret.MinStartsForPromotion = source.MinStartsForPromotion;
            ret.MinPointsForPromotion = source.MinPointsForPromotion;
            ret.PointsForFirst = source.PointsForFirst;
            ret.ExtraManualStarter = source.ExtraManualStarter;
            ret.Comment = source.Comment;
            ret.CompetitionColor = source.CompetitionColor;
            ret.Ignore = source.Ignore;

            return ret;
        }

        public static CompetitionClass? Map(
            this CompetitionClassViewModel source,
            CompetitionClass? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new CompetitionClass();

            ret.CompetitionId = source.CompetitionId;
            ret.CompetitionClassId = source.CompetitionClassId ?? Guid.Empty;
            ret.CompetitionClassName = source.CompetitionClassName;
            ret.FollowUpCompetitionClassId = source.FollowUpCompetitionClassId;
            ret.AdjudicatorPanelId = source.AdjudicatorPanelId;
            ret.OrgClassId = source.OrgClassId;
            ret.Discipline = source.Discipline;
            ret.AgeClass = source.AgeClass;
            ret.AgeGroup = source.AgeGroup;
            ret.Class = source.Class;
            ret.MinStartsForPromotion = source.MinStartsForPromotion;
            ret.MinPointsForPromotion = source.MinPointsForPromotion;
            ret.PointsForFirst = source.PointsForFirst;
            ret.ExtraManualStarter = source.ExtraManualStarter;
            ret.Comment = source.Comment;
            ret.CompetitionColor = source.CompetitionColor;
            ret.Ignore = source.Ignore;

            return ret;
        }

        #endregion CompetitionClass

        #region Competition

        public static CompetitionViewModel? Map(
            this Competition source,
            CompetitionViewModel? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new CompetitionViewModel();
            ret.CompetitionId = source.CompetitionId;
            ret.CompetitionName = source.CompetitionName;
            ret.Organization = source.Organization;
            ret.OrgCompetitionId = source.OrgCompetitionId;
            ret.CompetitionInfo = source.CompetitionInfo;
            ret.CompetitionDate = source.CompetitionDate;
            ret.Comment = source.Comment;

            return ret;
        }

        public static Competition? Map(
            this CompetitionViewModel source,
            Competition? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new Competition();
            ret.CompetitionId = source.CompetitionId ?? Guid.Empty;
            ret.CompetitionName = source.CompetitionName;
            ret.Organization = source.Organization;
            ret.OrgCompetitionId = source.OrgCompetitionId;
            ret.CompetitionInfo = source.CompetitionInfo;
            ret.CompetitionDate = source.CompetitionDate ?? DateTime.Now;
            ret.Comment = source.Comment;

            return ret;
        }

        #endregion CompetitionClass

        #region CompetitionVenue

        public static CompetitionVenueViewModel? Map(
            this CompetitionVenue source,
            CompetitionVenueViewModel? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new CompetitionVenueViewModel();
            ret.CompetitionId = source.CompetitionId;
            ret.CompetitionVenueId = source.CompetitionVenueId;
            ret.Name = source.Name;
            ret.LengthInMeter = source.LengthInMeter;
            ret.WidthInMeter = source.WidthInMeter;
            ret.Comment = source.Comment;

            return ret;
        }

        public static CompetitionVenue? Map(
            this CompetitionVenueViewModel source,
            CompetitionVenue? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new CompetitionVenue();
            ret.CompetitionId = source.CompetitionId;
            ret.CompetitionVenueId = source.CompetitionVenueId ?? Guid.Empty;
            ret.Name = source.Name;
            ret.LengthInMeter = source.LengthInMeter;
            ret.WidthInMeter = source.WidthInMeter;
            ret.Comment = source.Comment;

            return ret;
        }

        #endregion CompetitionClass

        #region ConfigurationValue

        public static ConfigurationViewModel? Map(
            this ConfigurationValue source,
            ConfigurationViewModel? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new ConfigurationViewModel();
            ret.Organization = source.Organization;
            ret.CompetitionId = source.CompetitionId;
            ret.CompetitionClassId = source.CompetitionClassId;
            ret.CompetitionVenueId = source.CompetitionVenueId;
            ret.Key = source.Key;
            ret.Value = source.Value;

            return ret;
        }

        public static ConfigurationValue? Map(
            this ConfigurationViewModel source,
            ConfigurationValue? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new ConfigurationValue();
            ret.Organization = source.Organization;
            ret.CompetitionId = source.CompetitionId;
            ret.CompetitionClassId = source.CompetitionClassId;
            ret.CompetitionVenueId = source.CompetitionVenueId;
            ret.Key = source.Key;
            ret.Value = source.Value;

            return ret;
        }

        #endregion ConfigurationValue

        #region ConfigurationValue

        public static ParticipantViewModel? Map(
            this Participant source,
            ParticipantViewModel? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new ParticipantViewModel();
            ret.CompetitionId = source.CompetitionId;
            ret.CompetitionClassId = source.CompetitionClassId;
            ret.ParticipantId = source.ParticipantId;
            ret.StartNumber = source.StartNumber;
            ret.NamePartA = source.NamePartA;
            ret.OrgIdPartA = source.OrgIdPartA;
            ret.NamePartB = source.NamePartB;
            ret.OrgIdPartB = source.OrgIdPartB;
            ret.ClubName = source.ClubName;
            ret.OrgIdClub = source.OrgIdClub;
            ret.OrgPointsPartA = source.OrgPointsPartA;
            ret.OrgStartsPartA = source.OrgStartsPartA;
            ret.MinStartsForPromotionPartA = source.MinStartsForPromotionPartA;
            ret.OrgAlreadyPromotedPartA = source.OrgAlreadyPromotedPartA;
            ret.OrgAlreadyPromotedInfoPartA = source.OrgAlreadyPromotedInfoPartA;
            ret.OrgPointsPartB = source.OrgPointsPartB;
            ret.OrgStartsPartB = source.OrgStartsPartB;
            ret.MinStartsForPromotionPartB = source.MinStartsForPromotionPartB;
            ret.OrgAlreadyPromotedPartB = source.OrgAlreadyPromotedPartB;
            ret.OrgAlreadyPromotedInfoPartB = source.OrgAlreadyPromotedInfoPartB;
            ret.Comment = source.Comment;
            ret.Ignore = source.Ignore;

            return ret;
        }

        public static Participant? Map(
            this ParticipantViewModel source,
            Participant? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new Participant();
            ret.CompetitionId = source.CompetitionId;
            ret.CompetitionClassId = source.CompetitionClassId ?? Guid.Empty;
            ret.ParticipantId = source.ParticipantId ?? Guid.Empty;
            ret.StartNumber = source.StartNumber;
            ret.NamePartA = source.NamePartA;
            ret.OrgIdPartA = source.OrgIdPartA;
            ret.NamePartB = source.NamePartB;
            ret.OrgIdPartB = source.OrgIdPartB;
            ret.ClubName = source.ClubName;
            ret.OrgIdClub = source.OrgIdClub;
            ret.OrgPointsPartA = source.OrgPointsPartA;
            ret.OrgStartsPartA = source.OrgStartsPartA;
            ret.MinStartsForPromotionPartA = source.MinStartsForPromotionPartA;
            ret.OrgAlreadyPromotedPartA = source.OrgAlreadyPromotedPartA;
            ret.OrgAlreadyPromotedInfoPartA = source.OrgAlreadyPromotedInfoPartA;
            ret.OrgPointsPartB = source.OrgPointsPartB;
            ret.OrgStartsPartB = source.OrgStartsPartB;
            ret.MinStartsForPromotionPartB = source.MinStartsForPromotionPartB;
            ret.OrgAlreadyPromotedPartB = source.OrgAlreadyPromotedPartB;
            ret.OrgAlreadyPromotedInfoPartB = source.OrgAlreadyPromotedInfoPartB;
            ret.Comment = source.Comment;
            ret.Ignore = source.Ignore;

            return ret;
        }

        #endregion ConfigurationValue
    }
}
