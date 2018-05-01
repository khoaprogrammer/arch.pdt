using CIF.DB;
using CIF.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CIF
{
    public class RuleCheckFilter : ActionFilterAttribute, IActionFilter
    {
        public string RuleName { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new { controller = "Account", action = "Login" })
                    );
            } else
            {
                string userId = filterContext.HttpContext.User.Identity.GetUserId();
                var user = DBHelper.GetUser(userId);
                var context = ApplicationDbContext.Create();
                var uManeger = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
                if (!context.Functions.Any(x => x.Id == this.RuleName))
                {
                    context.Functions.Add(new Function
                    {
                        Id = this.RuleName
                    });
                    context.SaveChanges();
                }
                if (!uManeger.IsInRole(userId, "ADMIN"))
                {
                    if (!user.Functions.Any(x => x.Id == RuleName))
                    {
                        filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary(new { controller = "Home", action = "Index" })
                        );
                    }
                }
            }
        }
    }
}