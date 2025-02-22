using PdfSharp.Fonts;

namespace DanceCompetitionHelper.Web.Helper.Documents
{
    public class UserFontResolver : IFontResolver
    {
        public byte[]? GetFont(string faceName)
        {
            // Buffering done by pdfsharp
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
            }

            // default...
            return null;
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
            }

            // default...
            return PlatformFontResolver.ResolveTypeface(
                familyName,
                bold,
                italic);
        }
    }
}
