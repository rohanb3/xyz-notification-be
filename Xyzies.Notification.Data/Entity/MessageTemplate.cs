using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xyzies.Notification.Data.Core;

namespace Xyzies.Notification.Data.Entity
{
    public class MessageTemplate: BaseEntity<Guid>
    {
        public DateTime CreateOn { get; set; }

        [Required]
        public string Cause { get; set; }

        public string Subject { get; set; }

        public string MessageBody { get; set; }

        public Guid TypeOfMessageId { get; set; }

        [ForeignKey(nameof(TypeOfMessageId))]
        public virtual TypeOfMessage TypeOfMessages { get; set; }
    }
}
