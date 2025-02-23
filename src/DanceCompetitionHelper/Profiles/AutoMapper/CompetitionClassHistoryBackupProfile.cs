using AutoMapper;

using DanceCompetitionHelper.Data.Backup;
using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Profiles.AutoMapper
{
    public class CompetitionClassHistoryBackupProfile : Profile
    {
        public CompetitionClassHistoryBackupProfile()
        {
            CreateMap<CompetitionClassHistory, CompetitionClassHistoryBackup>();
            CreateMap<CompetitionClassHistoryBackup, CompetitionClassHistory>();
        }
    }
}
