using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using System.IO;

namespace PrintQueueManager.Data
{
    public class OrdersImporter
    {
        public List<Order> Import(string file)
        {
            var fs = new System.IO.FileStream(file, FileMode.Open, FileAccess.Read);
            Byte[] imgByte = new byte[fs.Length];
            fs.Read(imgByte, 0, System.Convert.ToInt32(fs.Length));
            return Import(fs);

           

            


            var reader = System.IO.File.OpenText(file);
            var csv = new CsvReader(reader);
            csv.Configuration.HasHeaderRecord = true;
            var orderList = new List<Order>();
           // csv.Read(); // skip header.
            while (csv.Read())
            {
                try
                {
                    var order = new Order();

                    order.Sku = csv.GetField("SKU"); // (10);
                    order.Quantity = csv.GetField<int>(9);
                    order.OrderNotes = csv.GetField(3);
                    order.OrderId = csv.GetField(0);
                    order.CustomerEmail = csv.GetField(4);
                    order.Paid = csv.GetField(6) == "Paid";
                    order.Resend = csv.GetField(6) == "Resend";
                    order.ReceivedDate = csv.GetField<DateTime>(1);
                    order.CreatedDate = csv.GetField<DateTime>(7);
                    order.ItemTitle = csv.GetField(8);
                    order.CustomerName = csv.GetField(5);
                    order.ShortTitle = csv.GetField(11);
                    if (order.OrderNotes.Contains("Billing address was not provided by the channel") || !String.IsNullOrEmpty(order.OrderNotes)
                        || (order.Resend != null && (bool)order.Resend)
                        )
                        order.OrderStatus = OrderStatus.Paused;
                    //else if (String.IsNullOrEmpty( order.OrderNotes))
                    //    order.OrderStatus = OrderStatus.Paused;
                    else
                        order.OrderStatus = OrderStatus.Pending;
                    order.Linked = false;
                    orderList.Add(order);

                    
                }
                catch (Exception ex)
                {                  
                    Program.mainForm.log.AddMessage("Error importing or invalid format " + ex.Message);
                    break;
                }
            }

            return orderList;
        }

        public List<Order> Import(Stream stream)
        {
            stream.Position = 0;
            System.IO.TextReader reader = new System.IO.StreamReader(stream);
            
            //System.IO.TextReader.File.OpenText(file);
            var csv = new CsvReader(reader);
            csv.Configuration.HasHeaderRecord = true;
            var orderList = new List<Order>();
            csv.Read();
            csv.ReadHeader(); // skip header.
            while (csv.Read())
            {
                try
                {
                    var order = new Order();

                    order.Sku = csv.GetField("SKU");
                    order.Quantity = csv.GetField<int>("Quantity");
                    order.OrderNotes = csv.GetField("Order Notes");
                    order.OrderId = csv.GetField("Order Id");
                    order.CustomerEmail = csv.GetField("Customer email address");
                    order.Paid = csv.GetField("Paid") == "Paid";
                    order.Resend = csv.GetField("Paid") == "Resend";

                    
                    var receivedDate = csv.GetField("Received date");
                    var createdDate = csv.GetField("Created Date");
                    //18/12/2019 13:16
#if DEBUG
                    order.ReceivedDate = DateTime.ParseExact(receivedDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                    order.CreatedDate = DateTime.ParseExact(createdDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);

#else
                    if(receivedDate.Contains("-"))
                    {
                        order.ReceivedDate = DateTime.ParseExact(receivedDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                        order.CreatedDate = DateTime.ParseExact(createdDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        order.ReceivedDate = DateTime.ParseExact(receivedDate, "M/dd/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                        order.CreatedDate = DateTime.ParseExact(createdDate, "M/dd/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                    }
#endif

                    //try
                    //{
                    //    order.ReceivedDate = csv.GetField<DateTime>(receivedDate);
                    //    order.CreatedDate = csv.GetField<DateTime>(createdDate);
                    //}
                    //catch (Exception e)
                    //{
                    //    order.ReceivedDate = DateTime.ParseExact(receivedDate, "M/dd/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture); 
                    //    order.CreatedDate = DateTime.ParseExact(receivedDate, "M/dd/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture); 
                    //}

                    //order.ReceivedDate = csv.GetField<DateTime>(temp); // "Received date");
                    //order.CreatedDate = csv.GetField<DateTime>("Created Date");
                    order.ItemTitle = csv.GetField("Item title");
                    order.ShortTitle = csv.GetField("Short title");
                    order.CustomerName = csv.GetField("Shipping customer name");
                    order.PrintRun = csv.GetField("PRINT RUN");
                    if (order.OrderNotes.Contains("Billing address was not provided by the channel") || !String.IsNullOrEmpty(order.OrderNotes)
                            || (order.Resend != null && (bool)order.Resend) 
                            || (csv.GetField("Channel reference") == "MERGED")
                            )
                        order.OrderStatus = OrderStatus.Paused;
                    //else if (String.IsNullOrEmpty( order.OrderNotes))
                    //    order.OrderStatus = OrderStatus.Paused;
                    else
                        order.OrderStatus = OrderStatus.Pending;
                    order.Linked = false;
                    orderList.Add(order);


                }
                catch (Exception ex)
                {
                    Program.mainForm.log.AddMessage("Error importing or invalid format " + ex.Message);
                    continue; // break;
                }
            }

            return orderList;
        }


    }
}
