using CIF.DB;
using CIF.Filters;
using CIF.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CIF.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        [UserToViewBag]
        [ShowStats]
        public ActionResult Detail()
        {
            UserView user = UIHelper.GetUser(User.Identity.GetUserId());
            return View(user);
        }

        [HttpPost]
        [Authorize]
        public ActionResult CardDonate(CardDonateView model)
        {
            var user = DBHelper.GetUser(User.Identity.GetUserId());

            RequestInfo info = new RequestInfo();
            //Merchant site id
            info.Merchant_id = "54442";
            //Email tài khoản nhận tiền khi nạp
            info.Merchant_acount = "support@phongdaotao.com";
            //Mật khẩu giao tiếp khi đăng ký merchant site
            info.Merchant_password = "409397dd1da57ef08b0323f1d35dfdbb";

            //Nhà mạng
            switch (model.Type)
            {
                case CardType.VMS:
                    info.CardType = "VMS";
                    break;
                case CardType.VNP:
                    info.CardType = "VNP";
                    break;
                case CardType.VIETTEL:
                    info.CardType = "VIETTEL";
                    break;
                case CardType.GATE:
                    info.CardType = "GATE";
                    break;
                default:
                    break;
            }
            info.Pincard = model.PIN;

            //Mã đơn hàng
            info.Refcode = user.Email;
            info.SerialCard = model.Serial;

            ResponseInfo result = NLCardLib.CardChage(info);

            if (result.Errorcode == "00")
            {
                int gainCIF = 0;
                switch (result.Card_amount)
                {
                    case "20000":
                        gainCIF = 1000;
                        break;
                    case "50000":
                        gainCIF = 2500;
                        break;
                    case "100000":
                        gainCIF = 6000;
                        break;
                    case "200000":
                        gainCIF = 15000;
                        break;
                    case "500000":
                        gainCIF = 55000;
                        break;
                    default:
                        break;
                }
                DBHelper.AddUserCIF(user.Id, gainCIF);
            }

            TempData["Message"] = result.Message;

            return RedirectToAction("Donate", "Home");
        }

        [HttpPost]
        public ActionResult AdsFreeMonitor()
        {
            double remain = DBHelper.AdsFreeMonitor(User.Identity.GetUserId());
            return Json(remain, JsonRequestBehavior.AllowGet);
        }
    }
}