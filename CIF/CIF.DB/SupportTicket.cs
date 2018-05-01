using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIF.DB
{
    public enum SupportTickerType
    {
        [Display(Name = "Yêu cầu sách")]
        BOOK_REQUEST,
        [Display(Name = "Báo lỗi sách/web")]
        ERROR_REPORT,
        [Display(Name = "Liên hệ khác")]
        CONTACT
    }

    public class SupportTicket
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Detail { get; set; }
        public DateTime Time { get; set; }
        public SupportTickerType Type { get; set; }
    }
}
