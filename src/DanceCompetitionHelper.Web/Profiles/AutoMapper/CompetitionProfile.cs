using AutoMapper;
using DanceCompetitionHelper.Web.Models.CompetitionModels;
using Tables = DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Web.Profiles.AutoMapper
{
    public class CompetitionProfile : Profile
    {
        public CompetitionProfile()
        {
            CreateMap<CompetitionViewModel, Tables.Competition>();
            CreateMap<Tables.Competition, CompetitionViewModel>();
        }
    }
}
