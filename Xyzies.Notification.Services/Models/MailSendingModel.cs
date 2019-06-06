using SendGrid.Helpers.Mail;


namespace Xyzies.Notification.Services.Models
{
    public class MailSendingModel
    {
        public EmailAddress From { get; set; }
        public EmailAddress ReplyTo { get; set; }
        public string Subject { get; set; }
        public string PlainTextContent { get; set; }
        public string HtmlContent { get; set; }
    }
}
