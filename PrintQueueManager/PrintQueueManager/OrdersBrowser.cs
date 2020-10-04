using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintQueueManager
{
    using PrintQueueManager.Data;
    public partial class OrdersBrowser : Form
    {
        public OrdersBrowser()
        {
            InitializeComponent();

            this.DoubleBuffered = true;

            orderGridView.DoubleBuffered(true);
            PopulateProfilesCombo();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            BindGrid(txtSearch.Text);
        }

        private void BindGrid(string searchKey)
        {
            //CurrentList = db.ProfileOrders.Where(d => d.ProfileId == profile.Id)
            //                              .Skip(startPos).Take(noOfRecs).Select(d => new PrintItemDataRow
            var CurrentList = Program.mainForm.db.Orders.Where(d => d.Sku.Contains(searchKey) || d.OrderId.Contains(searchKey)).Select(d => new PrintItemDataRow
            {
                Quantity = d.Quantity,
                SKU = d.Sku,
                Linked = d.IsLinked ? "Yes" : "--",
                //PrintingStatus = d.OrderStatus == OrderStatus.Pending ? "Pending" : d.PrintingStatus.ToString(),
                TimeOfOrder = d.ReceivedDate,
                CustomerName = d.CustomerName,
                OrderNotes = d.OrderNotes,
                CustomerEmail = d.CustomerEmail,
                LinkedFileName = d.LinkedFilename,
                ShortTitle = d.ShortTitle,
                OrderId = d.OrderId,
                order = d,
                Remark = d.Remark,
                Id = d.Id,
                PrintRun = d.PrintRun,
            }).OrderByDescending(d => d.PrintingOrder)
              .ThenByDescending(d => d.TimeOfOrder)
              .ToList();

            orderGridView.DataSource = CurrentList;
            orderGridView.Refresh();
        }

        public Order GetOrder()
        {
            if (orderGridView.CurrentRow != null)
            {
                var item = (orderGridView.CurrentRow.DataBoundItem as PrintItemDataRow);
                if (item.Id == 0)
                {
                    Program.mainForm.db.Add(item.order);
                    Program.mainForm.db.SaveChanges();
                    //var order = Program.mainForm.db.Orders.Where(d => (d.OrderId == item.OrderId) && (d.Sku == item.SKU)).FirstOrDefault();
                }
                //else
                //{
                var order = Program.mainForm.db.Orders.Where(d => (d.OrderId == item.OrderId) && (d.Sku == item.SKU)).FirstOrDefault();
                if (order != null)
                    return order;
                else
                    MessageBox.Show("Order not found");
                //}
            }
            return null;
        }

        public int GetProfile()
        {
            if (cmbProfileList.SelectedIndex == 0)
                return 0;
            else
                return (int)cmbProfileList.SelectedValue;
        }

        public PrintItemDataRow GetSelectedData()
        {
            var item = (orderGridView.CurrentRow.DataBoundItem as PrintItemDataRow);
            return item;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists("import.csv"))
            {
                System.IO.FileStream fs = new System.IO.FileStream("import.csv",System.IO.FileMode.Open, System.IO.FileAccess.Read);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                fs.CopyTo(ms);
                var i = new OrdersImporter();
                var lst = i.Import(ms);

                var CurrentList = lst.Select(d => new PrintItemDataRow
                {
                    SKU = d.Sku,
                    OrderId = d.OrderId,
                    Quantity = d.Quantity,
                    Linked = d.IsLinked ? "Yes" : "--",
                    //PrintingStatus = d.OrderStatus == OrderStatus.Pending ? "Pending" : d.PrintingStatus.ToString(),
                    TimeOfOrder = d.ReceivedDate,
                    CustomerName = d.CustomerName,
                    OrderNotes = d.OrderNotes,
                    CustomerEmail = d.CustomerEmail,
                    LinkedFileName = d.LinkedFilename,
                    ShortTitle = d.ShortTitle,
                    Remark = d.Remark,
                    order = d,
                    Id = d.Id,
                    PrintRun = d.PrintRun,
                }).OrderByDescending(d => d.PrintingOrder)
              .ThenByDescending(d => d.TimeOfOrder)
              .ToList();

                orderGridView.DataSource = CurrentList;
                orderGridView.Refresh();

            }
        }

        private void PopulateProfilesCombo()
        {
            var lst = Program.mainForm.db.PrintProfiles.Select(d => new { ProfileName = d.ProfileName, Id = d.Id }).ToList();
            lst.Insert(0,new { ProfileName="",Id=0});
            cmbProfileList.DisplayMember = "ProfileName";
            cmbProfileList.ValueMember = "Id";
            cmbProfileList.DataSource = lst;
            cmbProfileList.SelectedIndex = 0;
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            if (orderGridView.DataSource==null)
                return;
            DateTime date;
            var CurrentList = (List<PrintItemDataRow>)orderGridView.DataSource;
            //List<PrintItemDataRow> result = null;
            var searchKey = txtSearch.Text;
            //var rec = CurrentList.Where(d => d.SKU == keyword); //.Contains(keyword));
            //rec = !rec.Any() ? CurrentList.Where(d => d.CustomerName == keyword) : rec;
            //rec = !rec.Any() ? CurrentList.Where(d => d.CustomerName.ContainsWord(keyword)) : rec;
            //rec = !rec.Any() ? CurrentList.Where(d => String.Equals(d.CustomerEmail, keyword, StringComparison.CurrentCultureIgnoreCase)) : rec;
            //result = rec.ToList();
            var result = CurrentList.Where(d => d.SKU.Contains(searchKey) || d.OrderId.Contains(searchKey)).ToList();
            orderGridView.DataSource = result;
            orderGridView.Refresh();
            //return result;
        }
    }
    
}
