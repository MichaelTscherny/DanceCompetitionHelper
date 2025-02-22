using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Web
{
    public static class WebConstants
    {
        public static class ViewData
        {
            public static class Parameter
            {
                public const string CompetitionName = "CompetitionName";
                public const string UseCompetitionClass = "Use" + nameof(CompetitionClass);
                public const string CurrentShowing = "CurrentShowing";
                // public const string Header = "LayoutNavigationHeader";
            }
        }
    }
}
