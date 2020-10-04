using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DitherMatrix_vs2010
{
   public partial class MatrixView : UserControl
   {
      public MatrixView()
      {
         InitializeComponent();
         UpdateMaxValues();
      }

      public int[][] Matrix
      {
         get { return matrix; }
         set
         {
            matrix = value;
            UpdateMaxValues();
            Invalidate();
         }
      }

      private void UpdateMaxValues()
      {
         maxMatrixWidth = 0;
         maxMatrixHeight = matrix.Length;

         for (int rowIndex = 0; rowIndex < matrix.Length; rowIndex++)
            maxMatrixWidth = Math.Max(matrix[rowIndex].Length, maxMatrixWidth);
      }

      protected override void OnPaint(PaintEventArgs e)
      {
         base.OnPaint(e);

         float currentYPos = 0;
         float lineHeight = e.Graphics.MeasureString("X", Font).Height;

         using (Brush brush = new SolidBrush(Color.Black))
         {
            for (int rowIndex = 0; rowIndex < maxMatrixHeight; rowIndex++)
            {
               StringBuilder sb = new StringBuilder();
               for (int columnIndex = 0; columnIndex < maxMatrixWidth; columnIndex++)
                  sb.AppendFormat("{0}\t", columnIndex < matrix[rowIndex].Length ? matrix[rowIndex][columnIndex] : 0);

               e.Graphics.DrawString(sb.ToString(), Font, brush, new PointF(0, currentYPos));
               currentYPos += lineHeight + lineHeight * 0.3f;
            }
         }
      }

      private int[][] matrix = new int[][] { new int[] { 0, 0 }, new int[] { 0, 0 } };
      private int maxMatrixWidth;
      private int maxMatrixHeight;
   }
}
