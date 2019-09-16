using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PrintQueueManager.Data
{
    public enum FilterType { ContainsA3=0,ContainsA5=1,ContainsA4=2,StartsD=3, StartsNH=4,StartsC=5, ContainsA2=6, ContainsA1=7 }

    [Table("PrintProfiles")]
    public partial class PrintProfile
    {
        public PrintProfile()
        {
            ProfileOrders = new HashSet<ProfileOrder>();
        }

        [Key]
        public int Id { get; set; }
        public bool AutoStartStop { get; set; }
        public string ProfileName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Status { get; set; }
        public string FtpServer { get; set; }
        public string FtpDirectory { get; set; }
        public string FtpUsername { get; set; }
        public string FtpPassword { get; set; }
        public DateTime? StartTime { get; set; }
        public int TotalCompletes { get; set; }
        public int TotalPendings { get; set; }
        public int TotalPauses { get; set; }
        public DateTime? EndTime { get; set; }
        public int? PresetId { get; set; }
        public DateTime? Eta { get; set; }
        public int FtpCheckInterval { get; set; }
        public FilterType FilterType { get; set; }
        public string SelectedPrinter { get; set; }
        public string PrintRun { get; set; }

        public ICollection<ProfileOrder> ProfileOrders { get; set; }
        public ICollection<Filter> ProfileFilters { get; set; }
    }
}
