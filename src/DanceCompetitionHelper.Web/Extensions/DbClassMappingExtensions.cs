﻿using DanceCompetitionHelper.Database.Enum;
using DanceCompetitionHelper.Database.Extensions;
using DanceCompetitionHelper.Database.Tables;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DanceCompetitionHelper.Web.Extensions
{
    public static class DbClassMappingExtensions
    {
        public static List<SelectListItem> ToSelectListItem(
            this IEnumerable<OrganizationEnum> organizations,
            OrganizationEnum? selectedItem = null,
            bool addEmpty = false)
        {
            var retList = new List<SelectListItem>();

            if (addEmpty)
            {
                retList.Add(
                    new SelectListItem()
                    {
                        Value = string.Empty,
                        Text = "None",
                        Selected = false,
                    });
            }

            if (organizations != null)
            {
                retList.AddRange(
                    organizations.Select(
                        x => new SelectListItem()
                        {
                            Value = x.ToString(),
                            Text = x.ToString(),
                            Selected = x == selectedItem
                        }));
            }

            return retList;
        }

        public static List<SelectListItem> ToSelectListItem(
            this IEnumerable<Competition> competitions,
            Guid? selectedItem = null,
            bool addEmpty = false)
        {
            var retList = new List<SelectListItem>();

            if (addEmpty)
            {
                retList.Add(
                    new SelectListItem()
                    {
                        Value = string.Empty,
                        Text = "None",
                        Selected = false
                    });
            }

            if (competitions != null)
            {
                retList.AddRange(
                    competitions.Select(
                        x => new SelectListItem()
                        {
                            Value = x.CompetitionId.ToString(),
                            Text = x.GetCompetitionName(),
                            Selected = x.CompetitionId == selectedItem
                        }));
            }

            return retList;
        }

        public static List<SelectListItem> ToSelectListItem(
            this IEnumerable<CompetitionClass> competitionClasses,
            Guid? selectedItem = null,
            bool addEmpty = false)
        {
            var retList = new List<SelectListItem>();

            if (addEmpty)
            {
                retList.Add(
                    new SelectListItem()
                    {
                        Value = string.Empty,
                        Text = "None",
                        Selected = false
                    });
            }

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
            this IEnumerable<CompetitionVenue> competitionVenues,
            Guid? selectedItem = null,
            bool addEmpty = false)
        {
            var retList = new List<SelectListItem>();

            if (addEmpty)
            {
                retList.Add(
                    new SelectListItem()
                    {
                        Value = string.Empty,
                        Text = "None",
                        Selected = false
                    });
            }

            if (competitionVenues != null)
            {
                retList.AddRange(
                    competitionVenues.Select(
                        x => new SelectListItem()
                        {
                            Value = x.CompetitionVenueId.ToString(),
                            Text = x.Name,
                            Selected = x.CompetitionVenueId == selectedItem
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
