using AutoMapper;
using DanceCompetitionHelper.Web.Models.CompetitionClassModels;
using Tables = DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Web.Profiles.AutoMapper
{
    public class CompetitionClassProfile : Profile
    {
        public CompetitionClassProfile()
        {
            CreateMap<CompetitionClassViewModel, Tables.CompetitionClass>();
            CreateMap<Tables.CompetitionClass, CompetitionClassViewModel>();
        }
    }
}
