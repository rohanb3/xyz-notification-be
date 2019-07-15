using System;
using System.ComponentModel.DataAnnotations;
using Xyzies.Notification.Data.Core;

namespace Xyzies.Notification.Data.Entity
{
    public class Log: BaseEntity<Guid>
    {
        [Required]
        public DateTime CreateOn { get; set; }

        [Required]
        public string Message { get; set; }

        public string Status { get; set; }
    }
}
