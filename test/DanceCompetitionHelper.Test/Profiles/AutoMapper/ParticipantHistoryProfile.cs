using AutoMapper;
using DanceCompetitionHelper.Database.Test.Pocos.DanceCompetitionHelper;
using Tables = DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Test.Profiles.AutoMapper
{
    public class ParticipantHistoryProfile : Profile
    {
        public ParticipantHistoryProfile()
        {
            CreateMap<ParticipantHistoryPoco, Tables.ParticipantHistory>();
        }
    }
}
