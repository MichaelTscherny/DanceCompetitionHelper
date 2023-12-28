using System.Drawing;

namespace DanceCompetitionHelper.Extensions
{
    public static class ColorExtensions
    {
        public static string ToRgbHexString(
            this Color color)
        {
            return string.Format(
                "#{0}{1}{2}",
                color.R.ToString("X2"),
                color.G.ToString("X2"),
                color.B.ToString("X2"));
        }
    }
}
