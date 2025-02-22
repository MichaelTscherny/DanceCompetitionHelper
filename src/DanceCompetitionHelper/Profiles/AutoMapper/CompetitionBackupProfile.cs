using AutoMapper;

using DanceCompetitionHelper.Data.Backup;
using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Profiles.AutoMapper
{
    public class CompetitionBackupProfile : Profile
    {
        public CompetitionBackupProfile()
        {
            CreateMap<Competition, CompetitionBackup>();
        }
    }
}
