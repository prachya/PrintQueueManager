using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using TallComponents.PDF.Rasterizer;
using System.Drawing.Drawing2D;
using TallComponents.PDF.Rasterizer.Configuration;
using System.Diagnostics;

namespace LayeredPDF
{
   public partial class MainForm : Form
   {
      private Document document = null;
      private int selectedPage = -1;
      private Bitmap bitmap = null;
      private LayerCollection layers = null;
      private bool suppresLayersUpdate = false;
      private const string sampleFileName = @"..\..\PDF\LayersDemo.pdf";

      [STAThread]
      static void Main()
      {
          Application.Run(new MainForm());
      }

      public MainForm()
      {
         InitializeComponent();

         open(sampleFileName);
      }

      private void OnOpenClick(object sender, EventArgs e)
      {
         close();

         OpenFileDialog dialog = new OpenFileDialog();
         dialog.Filter = "PDF Files|*.pdf|All files|*.*";
         if (DialogResult.OK == dialog.ShowDialog())
         {
            open(dialog.FileName);
         }
      }

      private void OnPageIndexChanged(object sender, EventArgs e)
      {
         int index = pagesListBox.SelectedIndex;
         if (index >= 0 && null != document && index < document.Pages.Count)
         {
            selectPage(index);
         }
      }

      private void OnLayerCheck(object sender, ItemCheckEventArgs e)
      {
         if (suppresLayersUpdate)
            return;

         //update visibility of a layer
         layers[e.Index].Visible = e.NewValue == CheckState.Checked;
         
         //update the current page
         selectPage(selectedPage);
      }

      private void selectPage(int index)
      {
         if (null == document) return;

         try
         {
            Cursor = Cursors.WaitCursor;

            selectedPage = index;
            Page page = document.Pages[selectedPage];

            if (null != bitmap)
            {
               bitmap.Dispose();
            }

            bitmap = new Bitmap((int)page.Width, (int)page.Height);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
               RenderSettings renderSettings = new RenderSettings();
               renderSettings.LayerSettings.Layers = layers;
               graphics.SmoothingMode = SmoothingMode.AntiAlias;
               page.Draw(graphics, renderSettings);
            }

            pictureBox1.Image = bitmap;
         }
         finally
         {
            Cursor = Cursors.Arrow;
         }
      }

      private void open(string path)
      {
         Debug.Assert(!String.IsNullOrEmpty(path), "The path to the PDF file is NULL or empty");
         Debug.Assert(File.Exists(path), "The specified file doesn't exist");

         using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
         {
            document = new Document(file);

            for (int i = 0; i < document.Pages.Count; i++)
            {
               pagesListBox.Items.Add("Page " + (i + 1));
            }

            layers = document.Layers;

            suppresLayersUpdate = true;
            foreach (Layer layer in layers)
            {
               layersListBox.Items.Add(layer.Name, layer.Visible);
            }
            suppresLayersUpdate = false;
         }
         this.pagesListBox.SelectedIndex = 0;
      }

      private void close()
      {
         document = null;
         if (null != bitmap)
         {
            bitmap.Dispose();
            bitmap = null;
         }
         this.pagesListBox.Items.Clear();
         this.layersListBox.Items.Clear();
      }
   }
}
