using CIF.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CIF.Controllers
{
    public class ShopController : Controller
    {
        [HttpPost]
        [Authorize]
        public ActionResult TimeExchange(int cif)
        {
            var user = DBHelper.GetUser(User.Identity.GetUserId());
            if (user.CIF < cif)
            {
                TempData["Message"] = "Tài khoản của bạn không đủ CIF";
            } else
            {
                DBHelper.TimeExchange(cif, user.Id);
                TempData["Message"] = "Giao dịch thành công. Chúc bạn có những giờ phút học tập vui vẻ!";
            }
            return RedirectToAction("Shop", "Home");
        }
    }
}