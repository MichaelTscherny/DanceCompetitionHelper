using DanceCompetitionHelper.Database;

namespace DanceCompetitionHelper.OrgImpl
{
    public interface ICompetitionImporter
    {
        List<string> ImportOrUpdateByFile(
            DanceCompetitionHelperDbContext dbCtx,
            string orgCompetitionId,
            string? fullPathCompetition,
            string? fullPathCompetitionClasses,
            string? fullPathParticipants);

        List<string> ImportOrUpdateByUrl(
            DanceCompetitionHelperDbContext dbCtx,
            string orgCompetitionId,
            Uri? uriCompetition,
            Uri? uriCompetitionClasses,
            Uri? uriParticipants);
    }
}
