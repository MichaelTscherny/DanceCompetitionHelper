using DanceCompetitionHelper.Database;

namespace DanceCompetitionHelper.OrgImpl
{
    public interface ICompetitionImporter
    {
        List<string> ImportOrUpdateByFile(
            DanceCompetitionHelperDbContext dbCtx,
            string? fullPathCompetition,
            string? fullPathCompetitionClasses,
            string? fullPathParticipants);

        List<string> ImportOrUpdateByUrl(
            DanceCompetitionHelperDbContext dbCtx,
            Uri? uriCompetition,
            Uri? uriCompetitionClasses,
            Uri? uriParticipants);
    }
}
