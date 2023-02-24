using DanceCompetitionHelper.Database;

namespace DanceCompetitionHelper.OrgImpl
{
    public interface ICompetitionImporter
    {
        void ImportByFile(
            DanceCompetitionHelperDbContext dbCtx,
            string fullPath);

        void ImportByUrl(
            DanceCompetitionHelperDbContext dbCtx,
            Uri fromUri);
    }
}
