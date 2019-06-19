using System;
using System.Collections.Generic;

namespace Xyzies.Notification.Services.Models
{
    public class EmailParameters
    {
        public string UDID { get; set; }

        public string Address { get; set; }

        public string Town { get; set; }

        public string PostCode { get; set; }

        public string Country { get; set; }

        public string Notes { get; set; }

        public DateTime LastHeartBeat { get; set; }

        public string Cause { get; set; }

        public string MailTo { get; set; }

        public IEnumerable<string> EmailsTo { get; set; }
    }
}
