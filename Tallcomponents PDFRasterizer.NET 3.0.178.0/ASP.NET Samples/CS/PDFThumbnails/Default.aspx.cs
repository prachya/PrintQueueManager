using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using TallComponents.Web.PDF;

namespace PdfThumbnails
{
   public partial class _Default : System.Web.UI.Page
   {
      protected Panel thumbnails;

      protected void Page_Load(object sender, EventArgs e)
      {

      }

      protected void DisplayThumbs_Click(object sender, System.EventArgs e)
      {
         thumbnails.Controls.Clear();

         HttpPostedFile postedFile = Request.Files["pdf"];
         if (null != postedFile)
         {
            byte[] buffer = new byte[postedFile.ContentLength];
            // There is actualy no need to reset the stream in C#, but in VB there is (don't know why)
            // therefore we reset the stream, so when converting this sample to VB it will also work
            postedFile.InputStream.Seek(0, System.IO.SeekOrigin.Begin);
            postedFile.InputStream.Read(buffer, 0, postedFile.ContentLength);
            string sessionID = Session.SessionID;
            Session[sessionID] = buffer;

            int n = Thumbnail.GetPageCount(postedFile.InputStream);

            for (int i = 1; i <= n; i++)
            {
               Thumbnail thumbnail = new Thumbnail();
               thumbnail.SessionKey = sessionID;
               thumbnail.Index = i;
               thumbnails.Controls.Add(thumbnail);
            }
         }
      }
   }
}
