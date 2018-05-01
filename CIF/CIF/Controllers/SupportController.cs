using CIF.Filters;
using CIF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CIF.Controllers
{
    public class SupportController : Controller
    {
        // GET: Support
        [UserToViewBag]
        [ShowStats]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(SupportTicketView model)
        {
            int id = DBHelper.AddSupportTicket(model);
            MailHelper.SendSupportConfirmation(model.Email, id);
            ViewBag.CompleteMessage = "Yêu cầu hỗ trợ của bạn đã được gửi đi thành công. Vui lòng chờ phản hồi qua email.";
            return View();
        }
    }
}