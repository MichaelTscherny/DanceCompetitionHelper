using AutoMapper;
using DanceCompetitionHelper.Test.Pocos.DanceCompetitionHelper;
using Tables = DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Test.Profiles.AutoMapper
{
    public class ConfigurationValueProfile : Profile
    {
        public ConfigurationValueProfile()
        {
            CreateMap<ConfigurationValuePoco, Tables.ConfigurationValue>();
        }
    }
}
