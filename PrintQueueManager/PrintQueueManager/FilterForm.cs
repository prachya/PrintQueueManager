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
            //grdStartsWith.Rows.Clear();
            foreach(var filterItem in filterData)
            {
                if (!string.IsNullOrEmpty( filterItem.StartsWith ))
                    grdStartsWith.Rows.Add(filterItem.StartsWith);
                if (!string.IsNullOrEmpty(filterItem.EndsWith))
                    grdEndsWith.Rows.Add(filterItem.EndsWith);
                if (!string.IsNullOrEmpty(filterItem.Contains))
                    grdContains.Rows.Add(filterItem.Contains);
                if (!string.IsNullOrEmpty(filterItem.DoesNotContain))
                    grdDoesnotContain.Rows.Add(filterItem.DoesNotContain);
            }
        }

        public void LoadLinkedSKU(IList<IgnoreLinkingText> lst)
        {
            foreach (var item in lst)
            {
                if (!string.IsNullOrEmpty(item.IgnoreText))
                    grdIgnoredText.Rows.Add(item.IgnoreText);
            }
        }

        public List<IgnoreLinkingText> GetLinkedSKUFilters()
        {
            var lst = new List<IgnoreLinkingText>();

            foreach (DataGridViewRow item in grdIgnoredText.Rows)
            {
                var row = new IgnoreLinkingText();

                if (!item.IsNewRow)
                {
                    row.IgnoreText = item.Cells[0].Value.ToString();
                    lst.Add(row);
                }
            }
            return lst;
        }

        public List<Filter> GetFilterData()
        {
            var lst = new List<Filter>();
            
            foreach (DataGridViewRow item in grdStartsWith.Rows)
            {
                var row = new Filter();
                
                if (!item.IsNewRow && (item.Cells["FilterValue"].Value != null))
                {
                    
                    if (item.Cells["FilterValue"].Value.ToString() != "")
                    {
                        row.StartsWith = item.Cells["FilterValue"].Value.ToString();
                        lst.Add(row);
                    }
                    
                }
            }

            var idx = 0;
            foreach (DataGridViewRow item in grdEndsWith.Rows)
            {
                if (!item.IsNewRow  && (item.Cells[0].Value != null) 
                                    && (item.Cells[0].Value.ToString() != ""))
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
                if (!item.IsNewRow && (item.Cells[0].Value != null)
                                   && (item.Cells[0].Value.ToString() != ""))
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
                if (!item.IsNewRow && (item.Cells[0].Value != null)
                                   && (item.Cells[0].Value.ToString() != ""))
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
            HandleDeleteKey(sender,e.KeyCode);
            
        }

        private void HandleDeleteKey(object grid,Keys code)
        {
            if (code == Keys.Delete)
            {
                var r = ((DataGridView)grid).CurrentRow; //.SelectedRows[0];
                if (r != null)
                    ((DataGridView)grid).Rows.Remove(r);

            }
        }

        private void GrdEndsWith_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void GrdEndsWith_KeyDown(object sender, KeyEventArgs e)
        {
            HandleDeleteKey(sender, e.KeyCode);
        }

        private void GrdContains_KeyDown(object sender, KeyEventArgs e)
        {
            HandleDeleteKey(sender, e.KeyCode);
        }

        private void GrdIgnoredText_KeyDown(object sender, KeyEventArgs e)
        {
            HandleDeleteKey(sender, e.KeyCode);
        }
    }
}
