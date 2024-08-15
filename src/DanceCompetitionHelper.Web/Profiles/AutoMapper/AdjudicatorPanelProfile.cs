using AutoMapper;
using DanceCompetitionHelper.Web.Models.AdjudicatorPanelModels;
using Tables = DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Web.Profiles.AutoMapper
{
    public class AdjudicatorPanelProfile : Profile
    {
        public AdjudicatorPanelProfile()
        {
            CreateMap<AdjudicatorPanelViewModel, Tables.AdjudicatorPanel>();
            CreateMap<Tables.AdjudicatorPanel, AdjudicatorPanelViewModel>();
        }
    }
}
