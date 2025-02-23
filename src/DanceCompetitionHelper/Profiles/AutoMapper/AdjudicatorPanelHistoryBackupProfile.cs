using AutoMapper;

using DanceCompetitionHelper.Data.Backup;
using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Profiles.AutoMapper
{
    public class AdjudicatorPanelHistoryBackupProfile : Profile
    {
        public AdjudicatorPanelHistoryBackupProfile()
        {
            CreateMap<AdjudicatorPanelHistory, AdjudicatorPanelHistoryBackup>();
            CreateMap<AdjudicatorPanelHistoryBackup, AdjudicatorPanelHistory>();
        }
    }
}
