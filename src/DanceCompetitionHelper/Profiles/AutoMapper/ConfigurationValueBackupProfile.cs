using AutoMapper;

using DanceCompetitionHelper.Data.Backup;
using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Profiles.AutoMapper
{
    public class ConfigurationValueBackupProfile : Profile
    {
        public ConfigurationValueBackupProfile()
        {
            CreateMap<ConfigurationValue, ConfigurationValueBackup>();
            CreateMap<ConfigurationValueBackup, ConfigurationValue>();
        }
    }
}
