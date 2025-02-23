using AutoMapper;

using DanceCompetitionHelper.Data.Backup;
using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Profiles.AutoMapper
{
    public class ParticipantBackupProfile : Profile
    {
        public ParticipantBackupProfile()
        {
            CreateMap<Participant, ParticipantBackup>();
            CreateMap<ParticipantBackup, Participant>();
        }
    }
}
