using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;

using TallComponents.PDF.Rasterizer;
using TallComponents.PDF.Rasterizer.Configuration;

namespace RenderFirstPage
{
   public partial class _Default : System.Web.UI.Page
   {
      bool renderCompleted = false;
      DateTime renderStart;
      DateTime renderEnd;
      System.Drawing.Imaging.ImageFormat imageFormat = null;

      protected Button ConvertPdf;
      protected DropDownList ListFormat;
      protected RadioButton Radio48;
      protected CheckBox CheckAntiAlias;

      protected void Page_Load(object sender, System.EventArgs e)
      {
         ConvertPdf.Attributes["onclick"] = "showUploadDialog(); ";

         if (Request.Params["image"] != null)
         {
            try
            {
               MemoryStream memory = ImageCache;
               if (memory != null)
               {
                  Response.Clear();
                  Response.ContentType = MetaType;
                  Response.AddHeader("Content-Length", memory.Length.ToString());
                  Response.OutputStream.Write(memory.GetBuffer(), 0, (int)memory.Length);
               }
               else
               {
                  Response.Status = "404";
               }
            }
            finally
            {
               // Do not call Response.Flush(). It may fail in non-full trust. Instead
               // use Reponse.End().
               Response.End();
               Response.Close();
            }
         }

         if (!IsPostBack)
         {
            ListFormat.Items.Add("PNG");
            ListFormat.Items.Add("JPEG");
            ListFormat.Items.Add("GIF");
         }

         switch (ListFormat.SelectedValue.ToLower())
         {
            case "jpeg":
               imageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
               MetaType = "image/jpeg";
               break;
            case "gif":
               imageFormat = System.Drawing.Imaging.ImageFormat.Gif;
               MetaType = "image/gif";
               break;
            default: // default is PNG
               imageFormat = System.Drawing.Imaging.ImageFormat.Png;
               MetaType = "image/png";
               break;
         }
      }

      protected void Page_PreRender(object sender, System.EventArgs e)
      {
         DataBind();
      }

      protected MemoryStream ImageCache
      {
         get { return (MemoryStream)Session["imageCache"]; }
         set { Session["imageCache"] = value; }
      }

      protected string MetaType
      {
         get { return (string)Session["metatype"]; }
         set { Session["metatype"] = value; }
      }

      protected TimeSpan RenderTime
      {
         get { return renderEnd.Subtract(renderStart); }
      }

      protected bool RenderCompleted
      {
         get { return renderCompleted; }
      }

      protected void Convert_Click(object sender, System.EventArgs e)
      {
         Bitmap bitmap = null;
         try
         {
            HttpPostedFile postedFile = Request.Files["pdf"];
            if (null != postedFile)
            {
               // get uploaded pdf data
               byte[] buffer = new byte[postedFile.ContentLength];
               postedFile.InputStream.Read(buffer, 0, buffer.Length);
               MemoryStream memory = new MemoryStream(buffer);

               // construct a document and retrieve the first page
               Document document = new Document(new BinaryReader(memory));
               TallComponents.PDF.Rasterizer.Page page = document.Pages[0];

               float scale = 1; // No transformation needed for 72 dpi
               if (Radio48.Checked) // scale to 48dpi
               {
                  scale = (float)48 / (float)72;
               }

               // draw the bitmap	
               bitmap = new Bitmap((int)(page.Width * scale), (int)(page.Height * scale));
               Graphics graphics = Graphics.FromImage(bitmap);
               if (scale != 1) // No transformation needed for 72 dpi
               {
                  graphics.ScaleTransform(scale, scale);
               }

               if (CheckAntiAlias.Checked)
               {
                  graphics.SmoothingMode = SmoothingMode.AntiAlias;
               }

               graphics.Clear(Color.White);

               TextRenderSettings.CMapFolder = Server.MapPath(".") + @"\CMaps\";

               RenderSettings renderSettings = new RenderSettings();
               renderSettings.ColorSettings.TransformationMode = ColorTransformationMode.HighQuality;
               renderSettings.TextSettings.FontSubstitutionMap.DefaultSubstitutionFont = "arialuni.ttf";
               renderSettings.TextSettings.FontSearchPath += ";" + Server.MapPath("Fonts");

               renderStart = DateTime.Now;
               page.Draw(graphics, renderSettings);
               renderEnd = DateTime.Now;

               // send bitmap to the memory stream
               memory = new MemoryStream();
               bitmap.Save(memory, imageFormat);
               
               // Store the image in cache
               ImageCache = memory;
               
               renderCompleted = true;
            }
         }
         catch (Exception exception)
         {
            Response.Write(exception.Message);
         }
         finally
         {
            if (null != bitmap) bitmap.Dispose();
         }
      }

      protected string GetProductAndVersionName(string assemblyName)
      {
         string product = "Unknown";
         string version = "Unknown";
         try
         {
            Assembly assembly = Assembly.LoadWithPartialName(assemblyName);
            version = assembly.GetName().Version.ToString();

            object[] objArray = assembly.GetCustomAttributes(false);
            foreach (object obj in objArray)
            {
               if (obj is AssemblyProductAttribute)
               {
                  AssemblyProductAttribute productAttr = obj as AssemblyProductAttribute;
                  product = productAttr.Product;
               }
            }
            return String.Format("{0} version {1}", product, version);
         }
         catch (Exception)
         {
            return String.Format("[Unable to load Assembly \"{0}\"]", assemblyName);
         }
      }
   }
}
