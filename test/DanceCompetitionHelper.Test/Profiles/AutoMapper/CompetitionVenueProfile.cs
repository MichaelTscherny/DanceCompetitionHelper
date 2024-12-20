using AutoMapper;
using DanceCompetitionHelper.Test.Pocos.DanceCompetitionHelper;
using Tables = DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Test.Profiles.AutoMapper
{
    public class CompetitionVenueProfile : Profile
    {
        public CompetitionVenueProfile()
        {
            CreateMap<CompetitionVenuePoco, Tables.CompetitionVenue>();
        }
    }
}
