namespace DanceCompetitionHelper.Extensions
{
    public static class EnumExtensions
    {
        public static IEnumerable<TEnum> GetValues<TEnum>(
            /* this TEnum forEnum */)
            where TEnum : System.Enum
        {
            foreach (TEnum curVal in Enum.GetValues(
                typeof(TEnum)))
            {
                yield return curVal;
            }
        }
    }
}
