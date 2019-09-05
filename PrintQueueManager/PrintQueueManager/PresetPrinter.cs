using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PrintQueueManager.Data
{

    [Table("PresetPrinters")]
    public partial class PresetPrinter
    {

        public PresetPrinter()
        {
            // ProfileOrders = new HashSet<ProfileOrder>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string PrinterName { get; set; }
        public int PresetId { get; set; }


        // public ICollection<ProfileOrder> ProfileOrders { get; set; }
    }
}
