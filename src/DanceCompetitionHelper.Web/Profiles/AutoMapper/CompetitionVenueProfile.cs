using AutoMapper;
using DanceCompetitionHelper.Web.Models.CompetitionVenueModels;
using Tables = DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Web.Profiles.AutoMapper
{
    public class CompetitionVenueProfile : Profile
    {
        public CompetitionVenueProfile()
        {
            CreateMap<CompetitionVenueViewModel, Tables.CompetitionVenue>();
            CreateMap<Tables.CompetitionVenue, CompetitionVenueViewModel>();
        }
    }
}
