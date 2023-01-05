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
    }
}
