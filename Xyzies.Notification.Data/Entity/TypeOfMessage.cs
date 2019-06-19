using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xyzies.Notification.Data.Core;

namespace Xyzies.Notification.Data.Entity
{
    public class TypeOfMessage : BaseEntity<Guid>
    {
        [Required]
        public string Type { get; set; }
    }
}
