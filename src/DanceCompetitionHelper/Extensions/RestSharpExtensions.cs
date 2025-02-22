using MigraDoc.DocumentObjectModel;

namespace DanceCompetitionHelper.Extensions
{
    public static class RestSharpExtensions
    {
        public static bool IsZeroOrEmpty(
            this Unit unit)
        {
            if (unit == null
                || Unit.Zero == unit
                || Unit.Empty == unit)
            {
                return true;
            }

            return false;
        }
    }
}
