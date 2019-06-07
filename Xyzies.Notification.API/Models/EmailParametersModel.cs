using System;

namespace Xyzies.Notification.API.Models
{
    public class EmailParametersModel
    {
        public string Udid { get; set; }

        public string Address { get; set; }

        public string Town { get; set; }

        public string PostCode { get; set; }

        public string Country { get; set; }

        public string Notes { get; set; }

        public DateTime LastHeartBeat { get; set; }
    }
}
