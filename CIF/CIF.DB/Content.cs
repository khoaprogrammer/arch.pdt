using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIF.DB
{
    public class Content
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string HTML { get; set; }
        public int BookId { get; set; }
        public int Order { get; set; }
        public virtual Book Book { get; set; }
        [InverseProperty("Content")]
        public virtual ICollection<TOCLine> TOCLines { get; set; }
    }
}
