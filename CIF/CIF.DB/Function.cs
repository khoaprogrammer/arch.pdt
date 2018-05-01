using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIF.DB
{
    public class Function
    {
        public string Id { get; set; }
        public virtual List<ApplicationUser> ApplicationUsers { get; set; }
    }
}
