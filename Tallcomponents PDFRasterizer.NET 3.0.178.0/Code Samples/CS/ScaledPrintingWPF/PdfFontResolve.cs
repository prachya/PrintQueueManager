using System.Diagnostics;
using TallComponents.PDF.Rasterizer.Configuration;
using TallComponents.PDF.Rasterizer.Fonts;

namespace ScaledPrintingWPF
{
    /// <summary>
    /// Get the most matching font
    /// </summary>
    public static class PdfFontResolve
    {
        #region FontResolving

        /// <summary>
        /// helper to resolve the fonts.
        /// </summary>
        /// <param name="sender">The render settings</param>
        /// <param name="args">The <see cref="TallComponents.PDF.Rasterizer.Fonts.ResolveFontEventArgs"/> instance containing the event data.</param>
        public static void Resolve(TextRenderSettings sender, ResolveFontEventArgs args)
        {
            Trace.WriteLine(string.Format("font ({0}, {1}, {2})", args.PdfFontName, args.SystemFontName, args.FontPath));

            if (args.FontLocation == FontLocation.System)
            {
                return;
            }

            // We try to map all fonts on to system fonts.
            // First handle a number of common extensions.
            string fontName = args.PdfFontName.ToLower();
             if (fontName.EndsWith("bold"))
            {
                args.Bold = true;
                if (fontName.EndsWith("-bold") || fontName.EndsWith(",bold"))
                {
                    fontName = args.PdfFontName.Remove(fontName.Length - 5);
                }
                else
                {
                    fontName = args.PdfFontName.Remove(fontName.Length - 4);
                }
            }
            else if (fontName.EndsWith("italic"))
            {
                args.Italic = true;
                if (fontName.EndsWith("-italic") || fontName.EndsWith(",italic"))
                {
                    fontName = args.PdfFontName.Remove(fontName.Length - 7);
                }
                else
                {
                    fontName = args.PdfFontName.Remove(fontName.Length - 6);
                }
            }
            else if (fontName.EndsWith("regular"))
            {
                if (fontName.EndsWith("-regular") || fontName.EndsWith(",regular"))
                {
                    fontName = args.PdfFontName.Remove(fontName.Length - 8);
                }
                else
                {
                    fontName = args.PdfFontName.Remove(fontName.Length - 7);
                }
            }
            else
            {
                fontName = args.PdfFontName;
            }

            switch (fontName)
            {
                case "TimesNewRoman":
                    args.SystemFontName = "Times New Roman";
                    break;

                case "LucidaConsole":
                    args.SystemFontName = "Lucida Console";
                    break;

                case "Univers-Condensed":
                    args.SystemFontName = "Univers Condensed Medium";
                    break;

                case "GillSansMT-Schlbk":
                    args.SystemFontName = "Gill Sans MT";
                    break;

                case "GillSansMT-SchlbkBold":
                    args.SystemFontName = "Gill Sans MT Bold";
                    break;

                case "GillSansMT-SchlbkItalic":
                    args.SystemFontName = "Gill Sans MT Italic";
                    break;

                case "Courier":
                    args.SystemFontName = "Courier New";
                    break;

                case "Times-Roman":
                    args.SystemFontName = "Times New Roman";
                    break;

                case "Times":
                    args.SystemFontName = "Times New Roman";
                    break;

                case "TimesNewRomanPSMT":
                    args.SystemFontName = "Times New Roman";
                    break;

                case "Helvetica":
                    args.SystemFontName = "Arial";
                    break;

                case "ArialNarrow":
                    args.SystemFontName = "Arial Narrow";
                    break;

                default:
                    args.SystemFontName = !args.Embedded ? "Arial Unicode MS" : args.PdfFontName;
                    break;
            }
        }

        #endregion //FontResolving
    }
}