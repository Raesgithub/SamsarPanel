using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class EmailService
    {
        public string SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                // اطلاعات حساب Gmail
                string fromEmail = "sameghbal0@gmail.com";
                string appPassword = "vfvi adap iupv drld"; // App Password نه رمز اصلی

                // تنظیمات SMTP
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromEmail, appPassword),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Timeout = 30000
                };

                // ایجاد ایمیل
                MailMessage mailMessage = new MailMessage()
                {
                    From = new MailAddress(fromEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true, // در صورت نیاز به HTML
                    Priority = MailPriority.Normal
                };

                mailMessage.To.Add(toEmail);

                // ارسال ایمیل
                smtpClient.Send(mailMessage);

                return "";
            }
            catch (Exception ex)
            {
                return $"خطا در ارسال ایمیل: {ex.Message}";
            }
        }
    }
}
