using AutoMapper;

using DanceCompetitionHelper.Data.Backup;
using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Profiles.AutoMapper
{
    public class AdjudicatorPanelBackupProfile : Profile
    {
        public AdjudicatorPanelBackupProfile()
        {
            CreateMap<AdjudicatorPanel, AdjudicatorPanelBackup>();
            CreateMap<AdjudicatorPanelBackup, AdjudicatorPanel>();
        }
    }
}
