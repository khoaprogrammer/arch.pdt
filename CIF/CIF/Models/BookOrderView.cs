using CIF.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIF.Models
{
    public class BookOrderView
    {
        public DateTime BuyDate { get; set; }
        public int BookId { get; set; }
        public string ApplicationUserId { get; set; }
        public  Book Book { get; set; }
        public  ApplicationUser ApplicationUser { get; set; }


      
    }
}