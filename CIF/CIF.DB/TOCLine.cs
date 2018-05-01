using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIF.DB
{
    public class TOCLine
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public int Order { get; set; }
        public int ContentId { get; set; }
        public string Anchor { get; set; }
        public virtual Content Content { get; set; }
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
        [NotMapped]
        public string DisplayPrefix {
            get
            {
                string result = "";
                for (int i = 0; i < this.Level; i++)
                {
                    result += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                }
                return result;
            }
            set { }
        }
    }
}
