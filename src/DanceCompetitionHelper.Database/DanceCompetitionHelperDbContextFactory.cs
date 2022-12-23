using DanceCompetitionHelper.Database.Config;
using Microsoft.EntityFrameworkCore.Design;

namespace DanceCompetitionHelper.Database
{
    public class DanceCompetitionHelperDbContextFactory : IDesignTimeDbContextFactory<DanceCompetitionHelperDbContext>
    {
        public DanceCompetitionHelperDbContext CreateDbContext(string[] args)
        {
            var useSqLiteDbFile = "dummyFile.sqlite";
            if (args != null
                && args.Length >= 1)
            {
                useSqLiteDbFile = args[0];
            }

            return new DanceCompetitionHelperDbContext(
                new SqLiteDbConfig()
                {
                    SqLiteDbFile = useSqLiteDbFile,
                },
                null);
        }
    }
}
