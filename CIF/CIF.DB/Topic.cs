using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIF.DB
{
    public class Topic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public virtual ICollection<Book> Books { get; set; }
        [ForeignKey("Parent")]
        public int? ParentId { get; set; }
        public virtual Topic Parent { get; set; }
        public virtual ICollection<Topic> Childs { get; set; }
        public virtual List<Document> Documents { get; set; }
    }
}
