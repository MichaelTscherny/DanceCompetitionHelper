namespace DanceCompetitionHelper.Web.Helper.Request
{
    [Flags]
    public enum SetOnEnum
    {
        OnSuccess = (1 << 0),
        OnError = (1 << 1),
        OnModelStateInvalid = (1 << 2),
        // OnNoData = (1 << 3),
    }
}
