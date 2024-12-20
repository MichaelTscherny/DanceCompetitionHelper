using AutoMapper;
using DanceCompetitionHelper.Database.Test.Pocos.DanceCompetitionHelper;
using Tables = DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Test.Profiles.AutoMapper
{
    public class CompetitionClassHistoryProfile : Profile
    {
        public CompetitionClassHistoryProfile()
        {
            CreateMap<CompetitionClassHistoryPoco, Tables.CompetitionClassHistory>();
        }
    }
}
