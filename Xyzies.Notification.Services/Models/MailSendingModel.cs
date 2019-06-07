using SendGrid.Helpers.Mail;
using System.Collections.Generic;

namespace Xyzies.Notification.Services.Models
{
    public class MailSendingModel
    {
        public MailSendingModel()
        {
            From = new EmailAddress();
            To = new List<EmailAddress>();
        }

        public EmailAddress From { get; set; }

        public List<EmailAddress> To { get; set; }

        public string Subject { get; set; }

        public string PlainTextContent { get; set; }

        public string HtmlContent { get; set; }

    }
}
