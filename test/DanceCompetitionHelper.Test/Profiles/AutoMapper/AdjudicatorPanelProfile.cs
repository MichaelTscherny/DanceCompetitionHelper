using AutoMapper;
using DanceCompetitionHelper.Test.Pocos.DanceCompetitionHelper;
using Tables = DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Test.Profiles.AutoMapper
{
    public class AdjudicatorPanelProfile : Profile
    {
        public AdjudicatorPanelProfile()
        {
            CreateMap<AdjudicatorPanelPoco, Tables.AdjudicatorPanel>();
        }
    }
}
