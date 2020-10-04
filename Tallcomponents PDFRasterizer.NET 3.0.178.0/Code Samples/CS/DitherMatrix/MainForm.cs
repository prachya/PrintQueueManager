using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using TallComponents.PDF.Rasterizer;
using System.Drawing.Drawing2D;

namespace DitherMatrix_vs2010
{
   public partial class MainForm : Form
   {
      Document document = null;

      //dithering matrices
      int[][][] matrices = new int[][][]{
         
         //matrix 0
         new int[][] { new int[] { 9, 9, 7 }, 
                       new int[] { 4, 1, 0 }, 
                       new int[]{ 0, 5, 2 } },
         
         //matrix 1
         new int[][] { new int[] { 5, 15 }, 
                       new int[] { 20, 10 } },

         //matrix 2
         new int[][] { new int[] { 30, 70, 40 }, 
                       new int[] { 60, 10, 90 }, 
                       new int[]{ 20, 80, 50 } },

         //matrix 3
         new int[][] { new int[] { 7, 2, 3 }, 
                       new int[] { 0, 4, 8 }, 
                       new int[]{ 5, 6, 1 } },
         
         //matrix 4
         new int[][] { new int[] { 8, 3, 4 }, 
                       new int[] { 6, 1, 2 }, 
                       new int[] { 7, 5, 9 } },

         //matrix 5
         new int[][] { new int[] { 1, 7, 4 }, 
                       new int[] { 5, 8, 3 }, 
                       new int[] { 6, 2, 9 } },

         //matrix 6
         new int[][] { new int[] { 1, 3}, 
                       new int[] { 4, 2 }},

         //matrix 7
         new int[][] { new int[] { 1, 9, 3 , 11}, 
                       new int[] { 13, 5, 15, 7 },
                       new int[] { 4, 12, 2, 10 }, 
                       new int[] { 16, 8, 14 , 6} },

         //matrix 8
         new int[][] { new int[] { 0, 8, 2, 10 }, 
                       new int[] { 12, 4, 14, 7 }, 
                       new int[] { 3, 11, 1, 9 }, 
                       new int[] { 15, 7, 13, 5 } },

         //matrix 9
         new int[][] { new int[] { 2, 6, 4 }, 
                       new int[] { 5, 0, 1 }, 
                       new int[] { 8, 3, 7 } },

         //matrix 10
         new int[][] { new int[] { 21, 10, 17, 14 , 23 }, 
                       new int[] { 15, 2, 6, 4, 9 }, 
                       new int[] { 20, 5, 0, 1 , 18 },
                       new int[] { 12, 8, 3, 7, 13 }, 
                       new int[] { 24, 18, 19, 11, 22 } },

         //matrix 11
         new int[][] { new int[] { 0, 2 }, 
                       new int[] { 2, 0 } },

         //matrix 12
         new int[][] { new int[] { 1, 9, 3 , 11}, 
                       new int[] { 13, 5, 15, 7 }, 
                       new int[] { 4, 12, 2, 10 }, 
                       new int[] { 16, 8, 14 , 6} }
      };

      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      static void Main()
      {
          Application.Run(new MainForm());
      }

      public MainForm()
      {
         InitializeComponent();
         FillOutMatrices();
      }

      private void FillOutMatrices()
      {
         int i = 0;
         for (i = 0; i < matrices.Length; i++)
            selectedMatrix.Items.Add(i);

         selectedMatrix.Items.Add("default");
         selectedMatrix.SelectedIndex = i;
      }

      private void OnSelectPDFDocumentClick(object sender, EventArgs e)
      {
         OpenFileDialog dialog = new OpenFileDialog();
         dialog.DefaultExt = ".pdf";
         if (DialogResult.OK == dialog.ShowDialog())
         {
            string path = dialog.FileName;
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
               document = new Document(file);

               selectedPage.Items.Clear();
               for (int i = 1; i <= document.Pages.Count; i++)
                  selectedPage.Items.Add(i);
               selectedPage.SelectedIndex = 0;
            }

            selectedPage.Enabled = true;
            selectedMatrix.Enabled = true;
            convertButton.Enabled = true;
         }
      }

      private void selectedMatrix_SelectedIndexChanged(object sender, EventArgs e)
      {
         matrixView1.Matrix = getDitherMatrix();
      }

      private void OnConvertClick(object sender, EventArgs e)
      {
         Page page = document.Pages[selectedPage.SelectedIndex];

         Bitmap bitmap = new Bitmap((int)page.Width, (int)page.Height);

         ConvertToTiffOptions options = new ConvertToTiffOptions();

         //set a dithering matrix
         options.DitherMatrix = getDitherMatrix();

         using (MemoryStream ms = new MemoryStream())
         {
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
               graphics.SmoothingMode = SmoothingMode.AntiAlias;
               graphics.Clear(Color.White);
               page.ConvertToTiff(ms, options);
            }
            bitmap = new Bitmap(ms, false);
         }

         pictureBox.Image = bitmap;
      }

      private int[][] getDitherMatrix()
      {
         if (selectedMatrix.SelectedIndex >= matrices.Length)
            //will force to use the default matrix
            return new int[0][] { };
         else
            return matrices[selectedMatrix.SelectedIndex];
      }
   }
}
