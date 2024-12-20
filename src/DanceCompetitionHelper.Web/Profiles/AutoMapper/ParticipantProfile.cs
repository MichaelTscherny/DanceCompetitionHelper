using AutoMapper;
using DanceCompetitionHelper.Web.Models.ParticipantModels;
using Tables = DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Web.Profiles.AutoMapper
{
    public class ParticipantProfile : Profile
    {
        public ParticipantProfile()
        {
            CreateMap<ParticipantViewModel, Tables.Participant>();
            CreateMap<Tables.Participant, ParticipantViewModel>();
        }
    }
}
