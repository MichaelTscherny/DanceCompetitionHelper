using DanceCompetitionHelper.Database;

namespace DanceCompetitionHelper.OrgImpl
{
    public interface ICompetitionImporter
    {
        void ImportOrUpdateByFile(
            DanceCompetitionHelperDbContext dbCtx,
            string? fullPathCompetition,
            string? fullPathCompetitionClasses,
            string? fullPathParticipants);

        void ImportOrUpdateByUrl(
            DanceCompetitionHelperDbContext dbCtx,
            Uri? uriCompetition,
            Uri? uriCompetitionClasses,
            Uri? uriParticipants);
    }
}
