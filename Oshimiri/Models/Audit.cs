using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oshimiri.Models
{
    public abstract class Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateOnly CreatedDate { get; set; }
        public DateOnly UpdatedDate { get; set; }
    }
}
