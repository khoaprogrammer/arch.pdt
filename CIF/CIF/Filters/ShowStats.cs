using CIF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CIF.Filters
{
    public class ShowStats : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var stats = DBHelper.GetStats();
            filterContext.Controller.ViewBag.AllBookCount = stats.Item1;
            filterContext.Controller.ViewBag.AllCourseCount = stats.Item2;
            filterContext.Controller.ViewBag.AllUserCount = stats.Item3;
        }
    }
}