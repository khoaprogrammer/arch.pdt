using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIF.DB
{
    public class Publisher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Introduction { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}
