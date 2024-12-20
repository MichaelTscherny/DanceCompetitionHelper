using AutoMapper;
using DanceCompetitionHelper.Web.Models.ConfigurationModels;
using Tables = DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Web.Profiles.AutoMapper
{
    public class ConfigurationProfile : Profile
    {
        public ConfigurationProfile()
        {
            CreateMap<ConfigurationViewModel, Tables.ConfigurationValue>();
            CreateMap<Tables.ConfigurationValue, ConfigurationViewModel>();
        }
    }
}
