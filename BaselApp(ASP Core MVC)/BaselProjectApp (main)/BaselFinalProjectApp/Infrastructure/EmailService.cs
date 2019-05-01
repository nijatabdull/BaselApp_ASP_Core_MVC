using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BaselFinalProjectApp.Infastructure
{
    public class EmailService
    {
        private readonly EmailServiceOption _option;
        public EmailService(IOptions<EmailServiceOption> emailServiceOption)
        {
            _option = emailServiceOption.Value;
        }

        public async Task SendEmailAsync(string toEmail,string subject,string message)
        {
            using (MailMessage mailMessage = new MailMessage())
            {
                using (SmtpClient smtpClient = new SmtpClient(_option.Host,_option.Port))
                {
                    mailMessage.From = new MailAddress(_option.Email,_option.DisplayName,System.Text.Encoding.UTF8);
                    mailMessage.To.Add(toEmail);
                    mailMessage.Subject = subject;
                    mailMessage.Body = message;

                    smtpClient.Credentials = new NetworkCredential(_option.Email,_option.Password);
                    smtpClient.EnableSsl = _option.EnableSSL;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
        }
    }

    public class EmailServiceOption
    {
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
    }
}
