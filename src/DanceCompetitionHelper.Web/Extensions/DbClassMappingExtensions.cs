using DanceCompetitionHelper.Database.Extensions;
using DanceCompetitionHelper.Database.Tables;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace DanceCompetitionHelper.Web.Extensions
{
    public static class DbClassMappingExtensions
    {
        public static async IAsyncEnumerable<SelectListItem> ToSelectListItemAsync(
            this IAsyncEnumerable<Competition> competitions,
            Guid? selectedItem = null,
            bool addEmpty = false)
        {
            if (addEmpty)
            {
                yield return new SelectListItem()
                {
                    Value = string.Empty,
                    Text = "None",
                    Selected = false
                };
            }

            await foreach (var curItem in competitions ?? AsyncEnumerable.Empty<Competition>())
            {
                yield return new SelectListItem()
                {
                    Value = curItem.CompetitionId.ToString(),
                    Text = curItem.GetCompetitionName(
                        true),
                    Selected = curItem.CompetitionId == selectedItem
                };
            }
        }

        public static async IAsyncEnumerable<SelectListItem> ToSelectListItemAsync(
            this IAsyncEnumerable<CompetitionClass> competitionClasses,
            Guid? selectedItem = null,
            bool addEmpty = false)
        {
            if (addEmpty)
            {
                yield return new SelectListItem()
                {
                    Value = string.Empty,
                    Text = "None",
                    Selected = false
                };
            }

            await foreach (var curItem in competitionClasses ?? AsyncEnumerable.Empty<CompetitionClass>())
            {
                yield return new SelectListItem()
                {
                    Value = curItem.CompetitionClassId.ToString(),
                    Text = curItem.GetCompetitionClassName(
                        true),
                    Selected = curItem.CompetitionClassId == selectedItem
                };
            }
        }

        public static async IAsyncEnumerable<SelectListItem> ToSelectListItemAsync(
            this IAsyncEnumerable<CompetitionVenue> competitionVenues,
            Guid? selectedItem = null,
            bool addEmpty = false)
        {
            if (addEmpty)
            {
                yield return new SelectListItem()
                {
                    Value = string.Empty,
                    Text = "None",
                    Selected = false
                };
            }

            await foreach (var curItem in competitionVenues ?? AsyncEnumerable.Empty<CompetitionVenue>())
            {
                yield return new SelectListItem()
                {
                    Value = curItem.CompetitionVenueId.ToString(),
                    Text = curItem.Name,
                    Selected = curItem.CompetitionVenueId == selectedItem
                };
            }
        }

        public static async IAsyncEnumerable<SelectListItem> ToSelectListItemAsync(
            this IAsyncEnumerable<AdjudicatorPanel> adjudicatorPanels,
            Guid? selectedItem = null)
        {
            await foreach (var curItem in adjudicatorPanels ?? AsyncEnumerable.Empty<AdjudicatorPanel>())
            {
                yield return new SelectListItem()
                {
                    Value = curItem.AdjudicatorPanelId.ToString(),
                    Text = curItem.Name,
                    Selected = curItem.AdjudicatorPanelId == selectedItem,
                };
            }
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
                            Selected = x.AdjudicatorId == selectedItem,
                        }));
            }

            return retList;
        }
    }
}
