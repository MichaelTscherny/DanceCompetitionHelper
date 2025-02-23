using AutoMapper;

using DanceCompetitionHelper.Data.Backup;
using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Profiles.AutoMapper
{
    public class AdjudicatorBackupHistoryProfile : Profile
    {
        public AdjudicatorBackupHistoryProfile()
        {
            CreateMap<AdjudicatorHistory, AdjudicatorHistoryBackup>();
            CreateMap<AdjudicatorHistoryBackup, AdjudicatorHistory>();
        }
    }
}
