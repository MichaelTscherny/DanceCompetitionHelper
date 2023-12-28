namespace DanceCompetitionHelper.OrgImpl
{
    public interface ICompetitionImporter
    {
        DanceCompetitionHelper? DanceCompetitionHelper { get; set; }

        List<string> ImportOrUpdateByFile(
            string orgCompetitionId,
            string? fullPathCompetition,
            string? fullPathCompetitionClasses,
            string? fullPathParticipants);

        List<string> ImportOrUpdateByUrl(
            string orgCompetitionId,
            Uri? uriUpdate,
            Uri? uriCompetition,
            Uri? uriCompetitionClasses,
            Uri? uriParticipants);
    }
}
