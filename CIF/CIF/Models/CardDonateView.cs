using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIF.Models
{
    public class CardDonateView
    {
        public string PIN { get; set; }
        public string Serial { get; set; }
        public CardType Type { get; set; }
    }
}