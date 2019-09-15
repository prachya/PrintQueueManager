using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PrintQueueManager.Data
{

    [Table("Presets")]
    public partial class Preset
    {

        public Preset()
        {
            // ProfileOrders = new HashSet<ProfileOrder>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string MediaType { get; set; }
        public string PresetName { get; set;}
        public string PrintQuality { get; set; } 
        public string PaperSize { get; set; }
        public string PaperSource { get; set; }
        public string OtherSettings { get; set; }


        // public ICollection<ProfileOrder> ProfileOrders { get; set; }
    }
}
