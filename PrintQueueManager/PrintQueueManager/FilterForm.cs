using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PrintQueueManager.Data;
namespace PrintQueueManager
{
    public partial class FilterForm : Form
    {
        
        public FilterForm()
        {
            InitializeComponent();
        }

        public void LoadData(IList<Filter> filterData)
        {
            foreach(var filterItem in filterData)
            {
                if (filterItem.StartsWith != "")
                    grdStartsWith.Rows.Add(filterItem.StartsWith);
                if (filterItem.EndsWith != "")
                    grdEndsWith.Rows.Add(filterItem.EndsWith);
                if (filterItem.Contains != "")
                    grdContains.Rows.Add(filterItem.Contains);
                if (filterItem.DoesNotContain != "")
                    grdDoesnotContain.Rows.Add(filterItem.DoesNotContain);
            }
        }

        public List<Filter> GetFilterData()
        {
            var lst = new List<Filter>();
            
            foreach (DataGridViewRow item in grdStartsWith.Rows)
            {
                var row = new Filter();
                
                if (!item.IsNewRow)
                {
                    row.StartsWith = item.Cells["FilterValue"].Value.ToString();
                    lst.Add(row);
                }
            }

            var idx = 0;
            foreach (DataGridViewRow item in grdEndsWith.Rows)
            {
                if (!item.IsNewRow)
                {
                    Filter row = null;
                    if (idx >= lst.Count())
                    {
                        row = new Filter();
                        lst.Add(row);
                    }
                    else
                        row = lst[idx];
                    row.EndsWith = item.Cells[0].Value.ToString();
                    ++idx;
                }
            }

            idx = 0;
            foreach (DataGridViewRow item in grdContains.Rows)
            {
                if (!item.IsNewRow)
                {
                    Filter row = null;
                    if (idx >= lst.Count())
                    {
                        row = new Filter();
                        lst.Add(row);
                    }
                    else
                        row = lst[idx];
                    row.Contains = item.Cells[0].Value.ToString();
                    ++idx;
                }
            }

            idx = 0;
            foreach (DataGridViewRow item in grdDoesnotContain.Rows)
            {
                if (!item.IsNewRow)
                {
                    Filter row = null;
                    if (idx >= lst.Count())
                    {
                        row = new Filter();
                        lst.Add(row);
                    }
                    else
                        row = lst[idx];
                    row.DoesNotContain = item.Cells[0].Value.ToString();
                    ++idx;
                }
            }

            return lst;
        }

        private void GrdStartsWith_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete)
            {
                var r = ((DataGridView)sender).CurrentRow; //.SelectedRows[0];
                if(r != null)
                    ((DataGridView)sender).Rows.Remove(r);
   
            }
        }
    }
}
