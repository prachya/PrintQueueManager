using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PrintQueueManager.Data
{
    [Table("Filters")]
    public partial class Filter
    {

        public Filter()
        {
            // ProfileOrders = new HashSet<ProfileOrder>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
        public string StartsWith { get; set; }
        public string EndsWith { get; set; }
        public string Contains { get; set; }
        public string DoesNotContain { get; set; }
        public int ProfileId { get; set; }
        [ForeignKey("ProfileId")]
        public PrintProfile Profile { get; set; }
    }
}
