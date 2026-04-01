using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.BLL.Service
{
   public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("jasminabualrub1999@gmail.com", "ywgt jxaa lugs rhan")
            };

            return client.SendMailAsync(
                new MailMessage(from: "jasminabualrub1999@gmail.com",
                                to: email,
                                subject,
                                message
                                )
                { IsBodyHtml = true });
        }
    }
}
