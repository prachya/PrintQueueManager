using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using TallComponents.PDF.Rasterizer;
using TallComponents.PDF.Rasterizer.Configuration;

namespace TallComponents.Web.PDF
{
	/// <summary>
	/// Summary description for ThumbnailPage.
	/// </summary>
	public class ThumbnailPage : System.Web.UI.Page
	{
      TallComponents.PDF.Rasterizer.Page getPage()
      {
         TextRenderSettings.CMapFolder = Server.MapPath( "." ) + @"\CMaps\";

         int index = int.Parse( Request.QueryString[Thumbnail.IndexKey] );
         if ( index < 1 ) index = 1;

         string path = Request.QueryString[ Thumbnail.PathKey ];
         string sessionKey = Request.QueryString[ Thumbnail.SessionKeyKey ];

         Stream stream = null;
         if ( null == path )
         {
            stream = new MemoryStream( Session[ sessionKey ] as byte[] );
         }
         else
         {
            FileStream file = null;
            try
            {
               file = new FileStream( Server.MapPath( path ), FileMode.Open, FileAccess.Read );
               byte[] buffer = new byte[ file.Length ];
               file.Read( buffer, 0, buffer.Length );
               stream = new MemoryStream( buffer );
            }
            finally
            {
               if ( null != file ) file.Close();
            }
         }

         Document document = new Document( new BinaryReader( stream ) );

         return document.Pages[ index - 1 ]; // 1-based index
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="writer"></param>
      protected override void Render( HtmlTextWriter writer )
      {
         System.Drawing.Image intermediateBitmap = null;
         int intermediateDpi = 72;
         try
         {
            TallComponents.PDF.Rasterizer.Page page = getPage();
            intermediateBitmap = new Bitmap( (int)page.Width, (int)page.Height );
            Graphics graphics = Graphics.FromImage( intermediateBitmap );
            graphics.Clear( Color.White );

            RenderSettings settings = new RenderSettings();
            if ( ( null != Request.QueryString[Thumbnail.HighQualityKey] ) && bool.Parse( Request.QueryString[Thumbnail.HighQualityKey] ) )
            {
               settings.ColorSettings.TransformationMode = ColorTransformationMode.HighQuality;
            }
            else
            {
               settings.ColorSettings.TransformationMode = ColorTransformationMode.HighSpeed;
            }
            if ( null != Request.QueryString[Thumbnail.DefaultFontKey] )
            {
               settings.TextSettings.FontSubstitutionMap.DefaultSubstitutionFont = Request.QueryString[Thumbnail.DefaultFontKey];
            }
            page.Draw( graphics, settings );
         }
         finally
         {
            Response.ContentType = "image/gif";
            Response.Cache.SetNoServerCaching();
            System.Drawing.Image image = null;
            try
            {
               int dpi = int.Parse( Request.QueryString[ Thumbnail.DpiKey ] );
               int borderWidth = getBorderWidth();
               if ( dpi > 18 ) dpi = 18;

               int thumbnailWidth = (int) ( ( intermediateBitmap.Width * dpi ) / intermediateDpi );
               int thumbnailHeight = (int) ( ( intermediateBitmap.Height * dpi ) / intermediateDpi );
               image = intermediateBitmap.GetThumbnailImage( thumbnailWidth, thumbnailHeight, null, new IntPtr() ); 
               Graphics graphics = Graphics.FromImage( image );
               graphics.DrawRectangle( new System.Drawing.Pen( getBorderColor(), borderWidth ), 0, 0, thumbnailWidth-1, thumbnailHeight-1 );
               using ( MemoryStream memoryStream = new MemoryStream() )
			   {
                  image.Save( memoryStream, ImageFormat.Gif );
                  memoryStream.WriteTo( Response.OutputStream );
			   }
            }
            finally
            {
               if ( null != intermediateBitmap ) intermediateBitmap.Dispose();
               if ( null != image ) image.Dispose();
            }
         }
      }

      Color getBorderColor()
      {
         string color = Request.QueryString[ Thumbnail.BorderColorKey ];
         byte r = byte.Parse( color.Substring( 0, 2 ), System.Globalization.NumberStyles.HexNumber );
         byte g = byte.Parse( color.Substring( 2, 2 ), System.Globalization.NumberStyles.HexNumber );
         byte b = byte.Parse( color.Substring( 4, 2 ), System.Globalization.NumberStyles.HexNumber );
         return Color.FromArgb( r, g, b );
      }

      int getBorderWidth()
      {
         return int.Parse( Request.QueryString[ Thumbnail.BorderWidthKey ] );
      }
	}
}
