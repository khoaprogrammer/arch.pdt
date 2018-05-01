using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIF.DB
{
    public class TranslatedContent
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string Code { get; set; }
        public int Location { get; set; }
        public string Data { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
