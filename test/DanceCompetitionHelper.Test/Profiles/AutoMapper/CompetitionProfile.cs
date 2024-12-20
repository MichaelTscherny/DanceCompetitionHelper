using AutoMapper;
using DanceCompetitionHelper.Database.Test.Pocos.DanceCompetitionHelper;
using DanceCompetitionHelper.Test.Bindings;
using Tables = DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Test.Profiles.AutoMapper
{
    public class CompetitionProfile : Profile
    {
        public CompetitionProfile()
        {
            CreateMap<CompetitionPoco, Tables.Competition>()
                .ForMember(
                    dest => dest.CompetitionDate,
                    opt => opt.NullSubstitute(BindingBase.UseNow));
        }
    }
}
