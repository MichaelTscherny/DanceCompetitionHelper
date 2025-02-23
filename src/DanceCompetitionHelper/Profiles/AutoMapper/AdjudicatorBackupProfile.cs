using AutoMapper;

using DanceCompetitionHelper.Data.Backup;
using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Profiles.AutoMapper
{
    public class AdjudicatorBackupProfile : Profile
    {
        public AdjudicatorBackupProfile()
        {
            CreateMap<Adjudicator, AdjudicatorBackup>();
            CreateMap<AdjudicatorBackup, Adjudicator>();
        }
    }
}
