using AutoMapper;
using DanceCompetitionHelper.Web.Models.AdjudicatorModels;
using Tables = DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Web.Profiles.AutoMapper
{
    public class AdjudicatorProfile : Profile
    {
        public AdjudicatorProfile()
        {
            CreateMap<AdjudicatorViewModel, Tables.Adjudicator>();
            CreateMap<Tables.Adjudicator, AdjudicatorViewModel>();
        }
    }
}
