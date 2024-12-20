using AutoMapper;
using DanceCompetitionHelper.Test.Pocos.DanceCompetitionHelper;
using Tables = DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Test.Profiles.AutoMapper
{
    public class AdjudicatorProfile : Profile
    {
        public AdjudicatorProfile()
        {
            CreateMap<AdjudicatorPoco, Tables.Adjudicator>();
        }
    }
}
