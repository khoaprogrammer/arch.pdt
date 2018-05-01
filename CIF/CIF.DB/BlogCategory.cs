using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIF.DB
{
    public class BlogCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<BlogCategory> Childs { get; set; }
        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual BlogCategory Parent { get; set; }
        [InverseProperty("Category")]
        public virtual List<BlogEntry> Entries { get; set; }
        public int Level { get; set; }
    }
}
