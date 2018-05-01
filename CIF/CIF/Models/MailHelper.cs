using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CIF.DB;
using MailKit.Net.Imap;
using MailKit;
using MailKit.Search;

namespace CIF.Models
{
    public class MailHelper
    {
        public static void SendSupportConfirmation(string to, int ticketId)
        {
            SupportTicket ticket = DBHelper.GetSupportTickets(ticketId);
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Support PDT", "support@phongdaotao.com"));
            message.To.Add(new MailboxAddress("Doe", to));
            message.Subject = "Hỗ trợ phongdaotao.com #" + ticketId;
            message.Body = new TextPart("html")
            {
                Text = @"Xin chào, <br>
<br>
Chúng tôi đã nhận được yêu cầu hỗ trợ của bạn. Chúng tôi sẽ liên lạc với bạn khi có tiến triển. <br>
Bạn có thể phản hồi email này để cung cấp thêm thông tin nhưng xin đừng thay đổi tiêu đề. <br>
<br>" +
"<b>Yêu cầu hỗ trợ số " + "#" + ticketId + "</b>" + @"<br>
<b>Ngày yêu cầu: </b>" + ticket.Time.ToString("HH:mm dd/MM/yyyy") + @"<br>
<b>Nội dung: </b>" + @"<br>
" + ticket.Detail + @"<br>
-- Support PDT"
            };

            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect(Globals.MailServer, Globals.SMTPPort, false);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("support@phongdaotao.com", "khoango123A");

                client.Send(message);
                client.Disconnect(true);
            }
        }

        public static List<TicketMessage> GetMessage(int ticketId)
        {
            SupportTicket ticket = DBHelper.GetSupportTickets(ticketId);

            using (var client = new ImapClient())
            {
                // For demo-purposes, accept all SSL certificates
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect(Globals.MailServer, Globals.IMAPPort, false);

                client.Authenticate("support@phongdaotao.com", "khoango123A");

                // The Inbox folder is always available on all IMAP servers...
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);
                var orderBy = new[] { OrderBy.Arrival };
                SearchQuery q = SearchQuery.SubjectContains("phongdaotao");

                List<TicketMessage> result = new List<TicketMessage>();

                var mails = inbox.Search(q);
                foreach (var mailId in mails)
                {
                    var message = inbox.GetMessage(mailId);
                    result.Add(new TicketMessage
                    {
                        Date = message.Date.UtcDateTime,
                        Detail = message.HtmlBody,
                        Sender = message.Sender != null ? message.Sender.Address : ""
                    });
                }

                client.Disconnect(true);

                return result.OrderByDescending(x => x.Date).ToList();
            }
        }
    }
}