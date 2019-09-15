using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PrintQueueManager.Data
{
    public enum OrderStatus { Unknown,Complete,Pending,Paused};

    [Table("Orders")]
    public partial class Order
    {
        public Order()
        {
            ProfileOrders = new HashSet<ProfileOrder>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime? TimeOfOrder { get; set; }
        public bool Linked { get; set; }

        [NotMapped]
        public bool IsLinked
        {
            get
            {
                return this.Linked || (!String.IsNullOrEmpty(this.LinkedFilename));
            }
        }

        public int Quantity { get; set; }
        public string OrderNotes { get; set; }
        public string OrderId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string Sku { get; set; }
        public bool Paid { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? Status { get; set; }
        public string ImportedFilename { get; set; }
        public DateTime? DateTimeImported { get; set; }
        public string LinkedFilename { get; set; }
        public bool ManualLinked { get; set; }
        public int? NoOfPages { get; set; }
        public bool? Resend { get; set; }
        public int? FileSize { get; set; }
        public DateTime? CompletedDateTime { get; set; }
        public string Remark { get; set; }
        public int? ReprintedQuantity { get; set; }
        public string ItemTitle { get; set; }
        public string ShortTitle { get; set; }
        public string PrintRun { get; set; }
        public ICollection<ProfileOrder> ProfileOrders { get; set; }
    }
}
