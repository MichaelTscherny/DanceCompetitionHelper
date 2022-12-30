namespace DanceCompetitionHelper.Database.Config
{
    public interface IDbConfig
    {
        string SqLiteDbFile { get; }

        bool LogAllSqls { get; }
        TimeSpan RuntimeInfo { get; }
        TimeSpan RuntimeWarn { get; }
        TimeSpan RuntimeError { get; }
    }
}
