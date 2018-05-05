
using CIF.DB;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIF.Models
{
    public class UserView
    {
        public UserView()
        {
            this.RoleIds = new List<string>();
        }
        public string Id { get; set; }
        public string Email { get; set; }
        public string PaswordHash { get; set; }
        public List<IdentityUserRole> Roles { get; set; }
        public List<string> RoleIds { get; set; }
        public List<Function> Functions { get; set; }
        public int Level { get; set; }
        public double EXP { get; set; }
        public double CIF { get; set; }
        public bool IsAdsFree { get; set; }
        public double TimeStorage { get; set; }
        public List<BookOrder> BookOrders { get; set; }
        public double PL
        {
            get
            {
                return 1 + ((this.Level) * Globals.PLIncreaseAmount);
            }
        }
        

        public bool InRole(string Id)
        {
            return this.Roles.Any(x => x.RoleId == Id);
        }

        public bool HasFunction(string Name)
        {
            return this.Functions.Any(x => x.Id == Name);
        }
    }
}