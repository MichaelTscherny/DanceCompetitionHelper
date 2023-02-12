using DanceCompetitionHelper.Database.Tables;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DanceCompetitionHelper.Web.Extensions
{
    public static class DbClassMappingExtensions
    {
        public static List<SelectListItem> ToSelectListItem(
            this IEnumerable<CompetitionClass> competitionClasses,
            Guid? selectedItem = null)
        {
            var retList = new List<SelectListItem>();

            if (competitionClasses != null)
            {
                retList.AddRange(
                    competitionClasses.Select(
                        x => new SelectListItem()
                        {
                            Value = x.CompetitionClassId.ToString(),
                            Text = x.CompetitionClassName,
                            Selected = x.CompetitionClassId == selectedItem
                        }));
            }

            return retList;
        }

        public static List<SelectListItem> ToSelectListItem(
            this IEnumerable<AdjudicatorPanel> adjudicatorPanels,
            Guid? selectedItem = null)
        {
            var retList = new List<SelectListItem>();

            if (adjudicatorPanels != null)
            {
                retList.AddRange(
                    adjudicatorPanels.Select(
                        x => new SelectListItem()
                        {
                            Value = x.AdjudicatorPanelId.ToString(),
                            Text = x.Name,
                            Selected = x.AdjudicatorPanelId == selectedItem
                        }));
            }

            return retList;
        }

        // TODO: needed?..
        public static List<SelectListItem> ToSelectListItem(
            this IEnumerable<Adjudicator> adjudicators,
            Guid? selectedItem = null)
        {
            var retList = new List<SelectListItem>();

            if (adjudicators != null)
            {
                retList.AddRange(
                    adjudicators.Select(
                        x => new SelectListItem()
                        {
                            Value = x.AdjudicatorId.ToString(),
                            Text = x.Name,
                            Selected = x.AdjudicatorId == selectedItem
                        }));
            }

            return retList;
        }
    }
}
