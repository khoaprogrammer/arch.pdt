using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIF.DB
{
    public class BookOrder
    {
        public DateTime BuyDate { get; set; }
        [Key, Column(Order =0)]
        public int BookId { get; set; }
        [Key, Column(Order = 1)]
        public string ApplicationUserId { get; set; }
        public virtual Book Book { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
