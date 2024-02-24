using AutoMapper;
using DanceCompetitionHelper.Web.Models.CompetitionModels;
using Tables = DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Web.Profiles.AutoMapper
{
    public class CompetitionProfile : Profile
    {
        public CompetitionProfile()
        {
            CreateMap<CompetitionViewModel, Tables.Competition>()
                .ForMember(
                    x => x.CompetitionDate,
                    opt => opt.NullSubstitute(DateTime.Now));
        }
    }
}
