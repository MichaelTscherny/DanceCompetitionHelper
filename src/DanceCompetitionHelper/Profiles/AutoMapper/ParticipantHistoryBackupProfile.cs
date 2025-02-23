using AutoMapper;

using DanceCompetitionHelper.Data.Backup;
using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Profiles.AutoMapper
{
    public class ParticipantHistoryBackupProfile : Profile
    {
        public ParticipantHistoryBackupProfile()
        {
            CreateMap<ParticipantHistory, ParticipantHistoryBackup>();
            CreateMap<ParticipantHistoryBackup, ParticipantHistory>();
        }
    }
}
