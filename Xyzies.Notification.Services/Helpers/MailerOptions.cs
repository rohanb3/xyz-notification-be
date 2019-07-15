
using System.Collections.Generic;

namespace Xyzies.Notification.Services.Helpers
{
    public class MailerOptions
    {
        public string ApiKey { get; set; }

        public string From { get; set; }

        public string MailTo { get; set; }

        public List<string> To { get; set; }


    }
}
