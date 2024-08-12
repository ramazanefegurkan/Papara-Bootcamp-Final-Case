using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.Notification
{
    public class EmailSender : INotificationSender
    {
        private readonly SmtpClient _smtpClient;

        public EmailSender(SmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
        }

        public async Task SendNotificationAsync(string name, string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Your App", "yourapp@example.com"));
            message.To.Add(new MailboxAddress(name, to));
            message.Subject = subject;
            message.Body = new TextPart("plain")
            {
                Text = body
            };

            await _smtpClient.SendAsync(message);
            await _smtpClient.DisconnectAsync(true);
        }
    }
}
