using AutoMapper;
using DanceCompetitionHelper.Database.Test.Pocos.DanceCompetitionHelper;
using Tables = DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Test.Profiles.AutoMapper
{
    public class ParticipantProfile : Profile
    {
        public ParticipantProfile()
        {
            CreateMap<ParticipantPoco, Tables.Participant>();
        }
    }
}
