using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using TallComponents.PDF.Rasterizer;
using TallComponents.PDF.Rasterizer.Configuration;

namespace TallComponents.Web.PDF
{
	/// <summary>
	/// Summary description for Thumbnail.
	/// </summary>
	public class Thumbnail : WebControl
	{
      /// <summary>
      /// Default constructor.
      /// </summary>
		public Thumbnail()
		{
		}

      static internal string DpiKey { get { return "dpi"; } }
      static internal string PathKey { get { return "path"; } }
      static internal string SessionKeyKey { get { return "sessionkey"; } }
      static internal string IndexKey { get { return "index"; } }
      static internal string BorderWidthKey { get { return "borderwidth"; } }
      static internal string BorderColorKey { get { return "bordercolor"; } }
      static internal string HighQualityKey { get { return "highquality"; } }
      static internal string DefaultFontKey { get { return "defaultfont"; } }

      string getThumbnailPageURL()
      {
         bool inSession = ( sessionKey != null ) && ( null != HttpContext.Current.Session[ sessionKey ] ) && ( HttpContext.Current.Session[ sessionKey ] is byte[] );
         
         return string.Format( 
            "{0}?{1}={2}&{3}={4}&{5}={6}&{7}={8}&{9}={10:x2}{11:x2}{12:x2}&{13}={14}&{15}={16}", 
            thumbnailPage,
            IndexKey, index,
            inSession ? SessionKeyKey : PathKey, 
            inSession ? sessionKey : HttpUtility.UrlEncode( path ),
            DpiKey, dpi,
            BorderWidthKey, borderWidth, 
            BorderColorKey, borderColor.R, borderColor.G, borderColor.B, 
            HighQualityKey, HighQuality,
            DefaultFontKey, DefaultFont
            );
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="writer"></param>
      protected override void Render( HtmlTextWriter writer )
      {
         if ( null != url )
         {
            writer.AddAttribute( "href", url );
            writer.RenderBeginTag( "a" );
         }
         
         writer.AddAttribute( "src", getThumbnailPageURL(), false );
         writer.AddAttribute( "border", "0", false );
         writer.RenderBeginTag( "img" );
         writer.RenderEndTag();

         if ( null != url )
         {
            writer.RenderEndTag();
         }
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="stream"></param>
      /// <returns></returns>
      public static int GetPageCount( System.IO.Stream stream )
      {
         Document document = new Document( new BinaryReader( stream ) );
         return document.Pages.Count;
      }

      /// <summary>
      /// 
      /// </summary>
      public string ThumbnailPage
      {
         get { return thumbnailPage; }
         set { thumbnailPage = value; }
      }

      /// <summary>
      /// 
      /// </summary>
      public int DPI
      {
         get { return dpi; }
         set { dpi = value; }
      }

      /// <summary>
      /// 
      /// </summary>
      public string Path
      {
         get { return path; }
         set { path = value; }
      }

      /// <summary>
      /// 
      /// </summary>
      public int Index
      {
         get { return index; }
         set { index = value; }
      }

      /// <summary>
      /// 
      /// </summary>
      public string SessionKey
      {
         get { return sessionKey; }
         set { sessionKey = value; }
      }

      /// <summary>
      /// 
      /// </summary>
      public string URL
      {
         get { return url; }
         set { url = value; }
      }

      /// <summary>
      /// 
      /// </summary>
      public new int BorderWidth
      {
         get { return borderWidth; }
         set { borderWidth = value; }
      }

      /// <summary>
      /// 
      /// </summary>
      public override System.Drawing.Color BorderColor
      {
         get { return borderColor; }
         set { borderColor = value; }
      }

      /// <summary>
      /// 
      /// </summary>
      public bool HighQuality
      {
         get { return highQuality == ColorTransformationMode.HighQuality; }
         set { highQuality = ( value ? ColorTransformationMode.HighQuality : ColorTransformationMode.HighSpeed ); }
      }

      /// <summary>
      /// 
      /// </summary>
      public string DefaultFont
      {
         get { return defaultFont; }
         set { defaultFont = value; }
      }

      string path = "sample.pdf";
      string sessionKey = null;
      int index;
      int dpi = 18;
      string thumbnailPage = "pdfthumbnail.aspx";
      string url = null;
      int borderWidth = 1;
      System.Drawing.Color borderColor = System.Drawing.Color.Gray;
      ColorTransformationMode highQuality = ColorTransformationMode.HighSpeed;
      string defaultFont = null;
	}
}
