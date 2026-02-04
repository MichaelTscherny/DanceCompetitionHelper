using DanceCompetitionHelper.Database.Test.Pocos.DanceCompetitionHelper;
using DanceCompetitionHelper.Test.Bindings;
using DanceCompetitionHelper.Test.Pocos.DanceCompetitionHelper;

using Tables = DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Test.Extensions
{
    internal static class TypeMapping
    {

        #region Tables.AdjudicatorPanel

        internal static Tables.AdjudicatorPanel? Map(
            this AdjudicatorPanelPoco source,
            Tables.AdjudicatorPanel? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new Tables.AdjudicatorPanel();
            ret.Name = source.Name;
            ret.Comment = source.Comment;

            return ret;
        }

        #endregion Tables.AdjudicatorPanel

        #region Tables.Adjudicator

        internal static Tables.Adjudicator? Map(
            this AdjudicatorPoco source,
            Tables.Adjudicator? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new Tables.Adjudicator();
            ret.Abbreviation = source.Abbreviation;
            ret.Name = source.Name;
            ret.Comment = source.Comment;

            return ret;
        }

        #endregion Tables.Adjudicator

        #region Tables.CompetitionClassHistory

        internal static Tables.CompetitionClassHistory? Map(
            this CompetitionClassHistoryPoco source,
            Tables.CompetitionClassHistory? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new Tables.CompetitionClassHistory();
            ret.OrgClassId = source.OrgClassId;
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
            ret.Ignore = source.Ignore;

            return ret;
        }

        #endregion Tables.CompetitionClassHistory

        #region Tables.CompetitionClass

        internal static Tables.CompetitionClass? Map(
            this CompetitionClassPoco source,
            Tables.CompetitionClass? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new Tables.CompetitionClass();
            ret.OrgClassId = source.OrgClassId;
            ret.CompetitionClassName = source.CompetitionClassName;
            ret.Discipline = source.Discipline;
            ret.AgeClass = source.AgeClass;
            ret.AgeGroup = source.AgeGroup;
            ret.Class = source.Class;
            ret.MinStartsForPromotion = source.MinStartsForPromotion ?? 0;
            ret.MinPointsForPromotion = source.MinPointsForPromotion ?? 0;
            ret.PointsForFirst = source.PointsForFirst ?? 0;
            ret.ExtraManualStarter = source.ExtraManualStarter;
            ret.Comment = source.Comment;
            ret.Ignore = source.Ignore;

            return ret;
        }

        #endregion Tables.CompetitionClass

        #region Tables.Competition

        internal static Tables.Competition? Map(
            this CompetitionPoco source,
            Tables.Competition? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new Tables.Competition();
            ret.CompetitionName = source.CompetitionName;
            ret.Organization = source.Organization;
            ret.OrgCompetitionId = source.OrgCompetitionId ?? string.Empty;
            ret.CompetitionInfo = source.CompetitionInfo;
            ret.CompetitionDate = source.CompetitionDate ?? BindingBase.UseNow;

            return ret;
        }

        #endregion Tables.Competition

        #region Tables.CompetitionVenue

        internal static Tables.CompetitionVenue? Map(
            this CompetitionVenuePoco source,
            Tables.CompetitionVenue? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new Tables.CompetitionVenue();
            ret.Name = source.Name;
            ret.Comment = source.Comment;

            return ret;
        }

        #endregion Tables.CompetitionClass

        #region Tables.ConfigurationValue

        internal static Tables.ConfigurationValue? Map(
            this ConfigurationValuePoco source,
            Tables.ConfigurationValue? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new Tables.ConfigurationValue();
            ret.Organization = source.Organization;
            ret.Key = source.Key;
            ret.Value = source.Value;

            return ret;
        }

        #endregion Tables.CompetitionClass

        #region Tables.ParticipantHistory

        internal static Tables.ParticipantHistory? Map(
            this ParticipantHistoryPoco source,
            Tables.ParticipantHistory? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new Tables.ParticipantHistory();
            ret.Version = source.Version;
            ret.StartNumber = source.StartNumber;
            ret.NamePartA = source.NamePartA;
            ret.OrgIdPartA = source.OrgIdPartA;
            ret.NamePartB = source.NamePartB;
            ret.OrgIdPartB = source.OrgIdPartB;
            ret.OrgIdClub = source.OrgIdClub;

            ret.OrgPointsPartA = source.OrgPointsPartA;
            ret.OrgStartsPartA = source.OrgStartsPartA;
            ret.OrgAlreadyPromotedPartA = source.OrgAlreadyPromotedPartA;
            ret.OrgAlreadyPromotedInfoPartA = source.OrgAlreadyPromotedInfoPartA;

            ret.OrgPointsPartB = source.OrgPointsPartB;
            ret.OrgStartsPartB = source.OrgStartsPartB;
            ret.OrgAlreadyPromotedPartB = source.OrgAlreadyPromotedPartB;
            ret.OrgAlreadyPromotedInfoPartB = source.OrgAlreadyPromotedInfoPartB;

            ret.Ignore = source.Ignore;

            return ret;
        }

        #endregion Tables.ParticipantHistory

        #region Tables.Participant

        internal static Tables.Participant? Map(
            this ParticipantPoco source,
            Tables.Participant? destination = null)
        {
            if (source == null)
            {
                return null;
            }

            var ret = destination ?? new Tables.Participant();
            ret.StartNumber = source.StartNumber;
            ret.NamePartA = source.NamePartA;
            ret.OrgIdPartA = source.OrgIdPartA;
            ret.NamePartB = source.NamePartB;
            ret.OrgIdPartB = source.OrgIdPartB;
            ret.OrgIdClub = source.OrgIdClub;
            ret.ClubName = source.ClubName;

            ret.OrgPointsPartA = source.OrgPointsPartA;
            ret.OrgStartsPartA = source.OrgStartsPartA;
            ret.OrgAlreadyPromotedPartA = source.OrgAlreadyPromotedPartA;
            ret.OrgAlreadyPromotedInfoPartA = source.OrgAlreadyPromotedInfoPartA;

            ret.OrgPointsPartB = source.OrgPointsPartB;
            ret.OrgStartsPartB = source.OrgStartsPartB;
            ret.OrgAlreadyPromotedPartB = source.OrgAlreadyPromotedPartB;
            ret.OrgAlreadyPromotedInfoPartB = source.OrgAlreadyPromotedInfoPartB;

            ret.Ignore = source.Ignore;

            return ret;
        }

        #endregion Tables.ParticipantHistory
    }
}
