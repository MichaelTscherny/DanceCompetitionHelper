using DanceCompetitionHelper.Data.Backup;
using DanceCompetitionHelper.Database.Tables;

using dbTypeMap = DanceCompetitionHelper.Database.Extensions.TypeMapping;

namespace DanceCompetitionHelper.Extensions
{
    public static class TypeMapping
    {
        #region Adjudicator

        public static AdjudicatorBackup? Map(
            this Adjudicator source,
            AdjudicatorBackup? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new AdjudicatorBackup();
            ret.AdjudicatorId = source.AdjudicatorId;
            ret.AdjudicatorPanelId = source.AdjudicatorPanelId;
            ret.Abbreviation = source.Abbreviation;
            ret.Name = source.Name;
            ret.Comment = source.Comment;

            return dbTypeMap.Map(source, ret);
        }

        public static Adjudicator? Map(
            this AdjudicatorBackup source,
            Adjudicator? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new Adjudicator();
            ret.AdjudicatorId = source.AdjudicatorId;
            ret.AdjudicatorPanelId = source.AdjudicatorPanelId;
            ret.Abbreviation = source.Abbreviation;
            ret.Name = source.Name;
            ret.Comment = source.Comment;

            return dbTypeMap.Map(source, ret);
        }

        public static AdjudicatorHistory? Map(
            this Adjudicator source,
            TableVersionInfo versionInfoCompetition,
            AdjudicatorHistory? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new AdjudicatorHistory();
            ret.AdjudicatorHistoryId = source.AdjudicatorId;
            ret.AdjudicatorPanelHistoryId = source.AdjudicatorPanelId;
            ret.Version = versionInfoCompetition.CurrentVersion;
            ret.Abbreviation = source.Abbreviation;
            ret.Name = source.Name;
            ret.Comment = source.Comment;

            // CAUTION: that's new...
            return ret;
        }

        #endregion Adjudicator

        #region AdjudicatorHistory

        public static AdjudicatorHistoryBackup? Map(
            this AdjudicatorHistory source,
            AdjudicatorHistoryBackup? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new AdjudicatorHistoryBackup();
            ret.AdjudicatorHistoryId = source.AdjudicatorHistoryId;
            ret.AdjudicatorPanelHistoryId = source.AdjudicatorPanelHistoryId;
            ret.Version = source.Version;
            ret.Abbreviation = source.Abbreviation;
            ret.Name = source.Name;
            ret.Comment = source.Comment;

            return dbTypeMap.Map(source, ret);
        }

        public static AdjudicatorHistory? Map(
            this AdjudicatorHistoryBackup source,
            AdjudicatorHistory? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new AdjudicatorHistory();

            ret.AdjudicatorHistoryId = source.AdjudicatorHistoryId;
            ret.AdjudicatorPanelHistoryId = source.AdjudicatorPanelHistoryId;
            ret.Version = source.Version;
            ret.Abbreviation = source.Abbreviation;
            ret.Name = source.Name;
            ret.Comment = source.Comment;

            return dbTypeMap.Map(source, ret);
        }

        #endregion AdjudicatorHistory

        #region AdjudicatorPanel

        public static AdjudicatorPanelBackup? Map(
            this AdjudicatorPanel source,
            AdjudicatorPanelBackup? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new AdjudicatorPanelBackup();
            ret.AdjudicatorPanelId = source.AdjudicatorPanelId;
            ret.CompetitionId = source.CompetitionId;
            ret.Name = source.Name;
            ret.Comment = source.Comment;

            return dbTypeMap.Map(source, ret);
        }

        public static AdjudicatorPanel? Map(
            this AdjudicatorPanelBackup source,
            AdjudicatorPanel? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new AdjudicatorPanel();
            ret.AdjudicatorPanelId = source.AdjudicatorPanelId;
            ret.CompetitionId = source.CompetitionId;
            ret.Name = source.Name;
            ret.Comment = source.Comment;

            return dbTypeMap.Map(source, ret);
        }

        public static AdjudicatorPanelHistory? Map(
            this AdjudicatorPanel source,
            TableVersionInfo versionInfoCompetition,
            AdjudicatorPanelHistory? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new AdjudicatorPanelHistory();
            ret.AdjudicatorPanelHistoryId = source.AdjudicatorPanelId;
            ret.CompetitionId = source.CompetitionId;
            ret.Version = versionInfoCompetition.CurrentVersion;
            ret.Name = source.Name;
            ret.Comment = source.Comment;

            // CAUTION: that's new...
            return ret;
        }

        #endregion AdjudicatorPanel

        #region AdjudicatorPanelHistory

        public static AdjudicatorPanelHistoryBackup? Map(
            this AdjudicatorPanelHistory source,
            AdjudicatorPanelHistoryBackup? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new AdjudicatorPanelHistoryBackup();
            ret.AdjudicatorPanelHistoryId = source.AdjudicatorPanelHistoryId;
            ret.CompetitionId = source.CompetitionId;
            ret.Version = source.Version;
            ret.Name = source.Name;
            ret.Comment = source.Comment;

            return dbTypeMap.Map(source, ret);
        }

        public static AdjudicatorPanelHistory? Map(
            this AdjudicatorPanelHistoryBackup source,
            AdjudicatorPanelHistory? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new AdjudicatorPanelHistory();
            ret.AdjudicatorPanelHistoryId = source.AdjudicatorPanelHistoryId;
            ret.CompetitionId = source.CompetitionId;
            ret.Version = source.Version;
            ret.Name = source.Name;
            ret.Comment = source.Comment;

            return dbTypeMap.Map(source, ret);
        }

        #endregion AdjudicatorPanelHistory

        #region Competition

        public static CompetitionBackup? Map(
            this Competition source,
            CompetitionBackup? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new CompetitionBackup();
            ret.CompetitionId = source.CompetitionId;
            ret.Organization = source.Organization;
            ret.OrgCompetitionId = source.OrgCompetitionId;
            ret.CompetitionName = source.CompetitionName;
            ret.CompetitionInfo = source.CompetitionInfo;
            ret.CompetitionDate = source.CompetitionDate;
            ret.Comment = source.Comment;

            return dbTypeMap.Map(source, ret);
        }

        public static Competition? Map(
            this CompetitionBackup source,
            Competition? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new Competition();
            ret.CompetitionId = source.CompetitionId;
            ret.Organization = source.Organization;
            ret.OrgCompetitionId = source.OrgCompetitionId;
            ret.CompetitionName = source.CompetitionName;
            ret.CompetitionInfo = source.CompetitionInfo;
            ret.CompetitionDate = source.CompetitionDate;
            ret.Comment = source.Comment;

            return dbTypeMap.Map(source, ret);
        }

        #endregion Competition

        #region CompetitionClass

        public static CompetitionClassBackup? Map(
            this CompetitionClass source,
            CompetitionClassBackup? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new CompetitionClassBackup();
            ret.CompetitionClassId = source.CompetitionClassId;
            ret.OrgClassId = source.OrgClassId;
            ret.CompetitionId = source.CompetitionId;
            ret.FollowUpCompetitionClassId = source.FollowUpCompetitionClassId;
            ret.AdjudicatorPanelId = source.AdjudicatorPanelId;
            ret.CompetitionClassName = source.CompetitionClassName;
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

            return dbTypeMap.Map(source, ret);
        }

        public static CompetitionClass? Map(
            this CompetitionClassBackup source,
            CompetitionClass? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new CompetitionClass();
            ret.CompetitionClassId = source.CompetitionClassId;
            ret.OrgClassId = source.OrgClassId;
            ret.CompetitionId = source.CompetitionId;
            ret.FollowUpCompetitionClassId = source.FollowUpCompetitionClassId;
            ret.AdjudicatorPanelId = source.AdjudicatorPanelId;
            ret.CompetitionClassName = source.CompetitionClassName;
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

            return dbTypeMap.Map(source, ret);
        }

        public static CompetitionClassHistory? Map(
            this CompetitionClass source,
            TableVersionInfo versionInfoCompetition,
            CompetitionClassHistory? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new CompetitionClassHistory();
            ret.CompetitionClassHistoryId = source.CompetitionClassId;
            ret.OrgClassId = source.OrgClassId;
            ret.CompetitionId = source.CompetitionId;
            ret.FollowUpCompetitionClassHistoryId = source.FollowUpCompetitionClassId;
            ret.AdjudicatorPanelHistoryId = source.AdjudicatorPanelId;
            ret.Version = versionInfoCompetition.CurrentVersion;
            ret.CompetitionClassName = source.CompetitionClassName;
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

            // CAUTION: that's new...
            return ret;
        }

        #endregion Competition

        #region CompetitionClassHistory

        public static CompetitionClassHistoryBackup? Map(
            this CompetitionClassHistory source,
            CompetitionClassHistoryBackup? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new CompetitionClassHistoryBackup();
            ret.CompetitionClassHistoryId = source.CompetitionClassHistoryId;
            ret.OrgClassId = source.OrgClassId;
            ret.CompetitionId = source.CompetitionId;
            ret.FollowUpCompetitionClassHistoryId = source.FollowUpCompetitionClassHistoryId;
            ret.AdjudicatorPanelHistoryId = source.AdjudicatorPanelHistoryId;
            ret.Version = source.Version;
            ret.CompetitionClassName = source.CompetitionClassName;
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

            return dbTypeMap.Map(source, ret);
        }

        public static CompetitionClassHistory? Map(
            this CompetitionClassHistoryBackup source,
            CompetitionClassHistory? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new CompetitionClassHistory();
            ret.CompetitionClassHistoryId = source.CompetitionClassHistoryId;
            ret.OrgClassId = source.OrgClassId;
            ret.CompetitionId = source.CompetitionId;
            ret.FollowUpCompetitionClassHistoryId = source.FollowUpCompetitionClassHistoryId;
            ret.AdjudicatorPanelHistoryId = source.AdjudicatorPanelHistoryId;
            ret.Version = source.Version;
            ret.CompetitionClassName = source.CompetitionClassName;
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

            return dbTypeMap.Map(source, ret);
        }

        #endregion CompetitionClassHistory

        #region CompetitionVenue

        public static CompetitionVenueBackup? Map(
            this CompetitionVenue source,
            CompetitionVenueBackup? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new CompetitionVenueBackup();
            ret.CompetitionVenueId = source.CompetitionVenueId;
            ret.CompetitionId = source.CompetitionId;
            ret.Name = source.Name;
            ret.LengthInMeter = source.LengthInMeter;
            ret.WidthInMeter = source.WidthInMeter;
            ret.Comment = source.Comment;

            return dbTypeMap.Map(source, ret);
        }

        public static CompetitionVenue? Map(
            this CompetitionVenueBackup source,
            CompetitionVenue? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new CompetitionVenue();
            ret.CompetitionVenueId = source.CompetitionVenueId;
            ret.CompetitionId = source.CompetitionId;
            ret.Name = source.Name;
            ret.LengthInMeter = source.LengthInMeter;
            ret.WidthInMeter = source.WidthInMeter;
            ret.Comment = source.Comment;

            return dbTypeMap.Map(source, ret);
        }

        public static CompetitionVenueHistory? Map(
            this CompetitionVenue source,
            TableVersionInfo versionInfoCompetition,
            CompetitionVenueHistory? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new CompetitionVenueHistory();
            ret.CompetitionVenueHistoryId = source.CompetitionVenueId;
            ret.CompetitionId = source.CompetitionId;
            ret.Version = versionInfoCompetition.CurrentVersion;
            ret.Name = source.Name;
            ret.LengthInMeter = source.LengthInMeter;
            ret.WidthInMeter = source.WidthInMeter;
            ret.Comment = source.Comment;

            // CAUTION: that's new...
            return ret;
        }

        #endregion CompetitionVenue

        #region ConfigurationValue

        public static ConfigurationValueBackup? Map(
            this ConfigurationValue source,
            ConfigurationValueBackup? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new ConfigurationValueBackup();

            ret.ConfigurationValueId = source.ConfigurationValueId;
            ret.Organization = source.Organization;
            ret.CompetitionId = source.CompetitionId;
            ret.CompetitionClassId = source.CompetitionClassId;
            ret.CompetitionVenueId = source.CompetitionVenueId;
            ret.Key = source.Key;
            ret.Value = source.Value;
            ret.Comment = source.Comment;

            return dbTypeMap.Map(source, ret);
        }

        public static ConfigurationValue? Map(
            this ConfigurationValueBackup source,
            ConfigurationValue? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new ConfigurationValue();
            ret.ConfigurationValueId = source.ConfigurationValueId;
            ret.Organization = source.Organization;
            ret.CompetitionId = source.CompetitionId;
            ret.CompetitionClassId = source.CompetitionClassId;
            ret.CompetitionVenueId = source.CompetitionVenueId;
            ret.Key = source.Key;
            ret.Value = source.Value;
            ret.Comment = source.Comment;

            return dbTypeMap.Map(source, ret);
        }

        public static ConfigurationValueHistory? Map(
            this ConfigurationValue source,
            TableVersionInfo versionInfoCompetition,
            ConfigurationValueHistory? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new ConfigurationValueHistory();
            ret.ConfigurationValueHistoryId = source.ConfigurationValueId;
            ret.Organization = source.Organization;
            ret.CompetitionId = source.CompetitionId;
            ret.CompetitionClassHistroyId = source.CompetitionClassId;
            ret.CompetitionVenueHistoryId = source.CompetitionVenueId;
            ret.Version = versionInfoCompetition.CurrentVersion;
            ret.Key = source.Key;
            ret.Value = source.Value;
            ret.Comment = source.Comment;

            return dbTypeMap.Map(source, ret);
        }

        #endregion ConfigurationValue

        #region Participant

        public static ParticipantBackup? Map(
            this Participant source,
            ParticipantBackup? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new ParticipantBackup();
            ret.ParticipantId = source.ParticipantId;
            ret.CompetitionId = source.CompetitionId;
            ret.CompetitionClassId = source.CompetitionClassId;
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

            return dbTypeMap.Map(source, ret);
        }

        public static Participant? Map(
            this ParticipantBackup source,
            Participant? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new Participant();
            ret.ParticipantId = source.ParticipantId;
            ret.CompetitionId = source.CompetitionId;
            ret.CompetitionClassId = source.CompetitionClassId;
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

            return dbTypeMap.Map(source, ret);
        }

        public static ParticipantHistory? Map(
            this Participant source,
            TableVersionInfo versionInfoCompetition,
            ParticipantHistory? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new ParticipantHistory();
            ret.ParticipantHistoryId = source.ParticipantId;
            ret.CompetitionId = source.CompetitionId;
            ret.CompetitionClassHistoryId = source.CompetitionClassId;
            ret.Version = versionInfoCompetition.CurrentVersion;
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

            // CAUTION: that's new...
            return ret;
        }

        #endregion Participant

        #region ParticipantHistory

        public static ParticipantHistoryBackup? Map(
            this ParticipantHistory source,
            ParticipantHistoryBackup? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new ParticipantHistoryBackup();
            ret.ParticipantHistoryId = source.ParticipantHistoryId;
            ret.CompetitionId = source.CompetitionId;
            ret.CompetitionClassHistoryId = source.CompetitionClassHistoryId;
            ret.Version = source.Version;
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

            return dbTypeMap.Map(source, ret);
        }

        public static ParticipantHistory? Map(
            this ParticipantHistoryBackup source,
            ParticipantHistory? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new ParticipantHistory();
            ret.ParticipantHistoryId = source.ParticipantHistoryId;
            ret.CompetitionId = source.CompetitionId;
            ret.CompetitionClassHistoryId = source.CompetitionClassHistoryId;
            ret.Version = source.Version;
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

            return dbTypeMap.Map(source, ret);
        }

        #endregion Participant
    }
}
