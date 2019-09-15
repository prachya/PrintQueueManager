using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PrintQueueManager.Data
{
    public enum PrintingStatus { Pending = 101, Printing = 102, Complete = 103 , Paused}

    [Table("ProfileOrders")]
    public partial class ProfileOrder
    {
        [Key]
        public int Id { get; set; }
        public int ProfileId { get; set; }
        public int OrderId { get; set; }
        public int? Reserved { get; set; }
        public string Reserved1 { get; set; }
        public int PrintingOrder { get; set; }
        public PrintingStatus PrintingStatus { get; set; }
        public DateTime? StaredPrintingTime { get; set; }
        public DateTime? CompletedPrintingTime { get; set; }
        public DateTime? DateTimeAdded { get; set; }
        public int? ItemStatus { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [ForeignKey("ProfileId")]
        public PrintProfile Profile { get; set; }
    }
}
