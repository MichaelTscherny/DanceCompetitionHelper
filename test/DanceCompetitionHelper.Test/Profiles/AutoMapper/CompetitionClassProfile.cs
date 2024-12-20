using AutoMapper;
using DanceCompetitionHelper.Database.Test.Pocos.DanceCompetitionHelper;
using Tables = DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Test.Profiles.AutoMapper
{
    public class CompetitionClassProfile : Profile
    {
        public CompetitionClassProfile()
        {
            CreateMap<CompetitionClassPoco, Tables.CompetitionClass>()
                .ForMember(
                    x => x.PointsForFirst,
                    opt => opt.NullSubstitute(0.0));
        }
    }
}
