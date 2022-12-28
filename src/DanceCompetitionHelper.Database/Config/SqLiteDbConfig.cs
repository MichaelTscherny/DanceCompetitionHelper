namespace DanceCompetitionHelper.Database.Config
{
    public class SqLiteDbConfig : IDbConfig
    {
        public const string Name = "DbConfig";

        public string SqLiteDbFile { get; init; } = default!;

        public bool LogAllSqls { get; init; } = false;
        public TimeSpan RuntimeInfo { get; init; } = TimeSpan.FromMilliseconds(80);
        public TimeSpan RuntimeWarn { get; init; } = TimeSpan.FromMilliseconds(120);
        public TimeSpan RuntimeError { get; init; } = TimeSpan.FromMilliseconds(250);
    }
}
