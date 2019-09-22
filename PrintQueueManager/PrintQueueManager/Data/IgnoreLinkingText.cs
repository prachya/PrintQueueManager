using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PrintQueueManager.Data
{
    [Table("IgnoreLinkingTexts")]
    public partial class IgnoreLinkingText
    {

        public IgnoreLinkingText()
        {
            // ProfileOrders = new HashSet<ProfileOrder>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string IgnoreText { get; set; }
        
        // [ForeignKey("PrintProfile")]
        public int ProfileId { get; set; }

        [ForeignKey("ProfileId")]
        public PrintProfile Profile { get; set; }
    }
}
