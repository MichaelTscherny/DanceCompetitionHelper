using AutoMapper;

using DanceCompetitionHelper.Data.Backup;
using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Profiles.AutoMapper
{
    public class CompetitionVenueBackupProfile : Profile
    {
        public CompetitionVenueBackupProfile()
        {
            CreateMap<CompetitionVenue, CompetitionVenueBackup>();
            CreateMap<CompetitionVenueBackup, CompetitionVenue>();
        }
    }
}
