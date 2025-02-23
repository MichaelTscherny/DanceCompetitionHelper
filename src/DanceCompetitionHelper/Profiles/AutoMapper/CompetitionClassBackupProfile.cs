using AutoMapper;

using DanceCompetitionHelper.Data.Backup;
using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Profiles.AutoMapper
{
    public class CompetitionClassBackupProfile : Profile
    {
        public CompetitionClassBackupProfile()
        {
            CreateMap<CompetitionClass, CompetitionClassBackup>();
        }
    }
}
