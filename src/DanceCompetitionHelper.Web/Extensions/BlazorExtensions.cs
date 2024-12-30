using DanceCompetitionHelper.Extensions;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace DanceCompetitionHelper.Web.Extensions
{
    public static class BlazorExtensions
    {
        public static IEnumerable<SelectListItem> ToSelectListItems<TEnum>(
            this IEnumerable<TEnum> enumItemList,
            TEnum selectedItem,
            bool addEmpty = false,
            string emptyText = "None")
            where TEnum : Enum
        {
            if (addEmpty)
            {
                yield return new SelectListItem()
                {
                    Value = string.Empty,
                    Text = emptyText ?? "None",
                    Selected = false,
                };
            }

            foreach (var curItem in enumItemList ?? Enumerable.Empty<TEnum>())
            {
                yield return new SelectListItem()
                {
                    Value = curItem.ToString(),
                    Text = curItem.ToString(),
                    Selected = curItem.Equals(
                        selectedItem),
                };
            }
        }

        public static IEnumerable<SelectListItem> ToSelectListItems<TEnum>(
            TEnum selectedItem,
            bool addEmpty = false,
            string emptyText = "None")
            where TEnum : Enum
        {
            if (typeof(TEnum) == null)
            {
                yield break;
            }

            var enumValues = EnumExtensions
                .GetValues<TEnum>();

            if (addEmpty)
            {
                yield return new SelectListItem()
                {
                    Value = string.Empty,
                    Text = emptyText ?? "None",
                    Selected = false,
                };
            }

            foreach (var curItem in enumValues)
            {
                yield return new SelectListItem()
                {
                    Value = curItem.ToString(),
                    Text = curItem.ToString(),
                    Selected = curItem.Equals(
                        selectedItem),
                };
            }
        }
    }
}
