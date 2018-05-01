using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIF.DB
{
    public class BlogEntry
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PostTime { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public string PreviewImageUrl { get; set; }
        [InverseProperty("Entries")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual BlogCategory Category { get; set; }
        public int ReadCount { get; set; }
    }
}
