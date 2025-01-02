using PdfSharp.Fonts;

namespace DanceCompetitionHelper.Web.Helper.Documents
{
    public class UserFontResolver : IFontResolver
    {
        public byte[]? GetFont(string faceName)
        {
            switch (faceName.ToLower())
            {
                case "arial narrow":
                case "arial narrow#":
                    return File.ReadAllBytes(
                        @"wwwroot\fonts\arialnarrow.ttf");

                case "arial narrow#b":
                    return File.ReadAllBytes(
                        @"wwwroot\fonts\arialnarrow_bold.ttf");

                case "arial narrow#bi":
                    return File.ReadAllBytes(
                        @"wwwroot\fonts\arialnarrow_bolditalic.ttf");

                case "arial narrow#i":
                    return File.ReadAllBytes(
                        @"wwwroot\fonts\arialnarrow_italic.ttf");

                // ------
                case "segoe ui this":
                case "segoe ui this#":
                    return File.ReadAllBytes(
                        @"wwwroot\fonts\segoeuithis.ttf");

                case "segoe ui this#b":
                    return File.ReadAllBytes(
                        @"wwwroot\fonts\segoeuithibd.ttf");

                case "segoe ui this#bi":
                    return File.ReadAllBytes(
                        @"wwwroot\fonts\segoeuithisi.ttf");

                case "segoe ui this#i":
                    return File.ReadAllBytes(
                        @"wwwroot\fonts\segoeuithisz.ttf");

                // ------
                case "segoe ui":
                case "segoe ui#":
                    return File.ReadAllBytes(
                        @"wwwroot\fonts\Segoe UI.ttf");

                case "segoe ui#b":
                    return File.ReadAllBytes(
                        @"wwwroot\fonts\Segoe UI Bold.ttf");

                case "segoe ui#bi":
                    return File.ReadAllBytes(
                        @"wwwroot\fonts\Segoe UI Bold Italic.ttf");

                case "segoe ui#i":
                    return File.ReadAllBytes(
                        @"wwwroot\fonts\Segoe UI Italic.ttf");

                // ------
                case "noto sans":
                    return File.ReadAllBytes(
                        @"wwwroot\fonts\NotoSansMayanNumerals-Regular.ttf");

                case "formiena":
                    return File.ReadAllBytes(
                        @"wwwroot\fonts\formiena.ttf");

                case "fs-regulate":
                    return File.ReadAllBytes(
                        @"wwwroot\fonts\fs-regulate.ttf");
            }

            // default...
            return GetFont(
                faceName);
        }

        public FontResolverInfo? ResolveTypeface(
            string familyName,
            bool bold,
            bool italic)
        {
            var prefix = "#";

            if (bold)
            {
                prefix += "b";
            }

            if (italic)
            {
                prefix += "i";
            }

            switch (familyName.ToLower())
            {
                case "arial narrow":
                    return new FontResolverInfo(
                        string.Format(
                            "Arial Narrow{0}",
                            prefix));

                case "segoe ui":
                    return new FontResolverInfo(
                        string.Format(
                            "Segoe UI{0}",
                            prefix));

                case "segoe ui this":
                    return new FontResolverInfo(
                        string.Format(
                            "Segoe UI This{0}",
                            prefix));

                case "noto sans":
                    return new FontResolverInfo(
                        string.Format(
                            "Noto Sans",
                            prefix));

                case "formiena":
                    return new FontResolverInfo(
                        string.Format(
                            "formiena",
                            prefix),
                        true,
                        true);

                case "fs-regulate":
                    return new FontResolverInfo(
                        string.Format(
                            "fs-regulate",
                            prefix),
                        true,
                        true);
            }

            // default...
            return PlatformFontResolver.ResolveTypeface(
                familyName,
                bold,
                italic);
        }
    }
}
