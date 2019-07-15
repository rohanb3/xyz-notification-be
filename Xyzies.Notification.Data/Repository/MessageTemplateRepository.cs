using System;
using Xyzies.Notification.Data.Entity;
using Xyzies.Notification.Data.Repository.Behaviour;

namespace Xyzies.Notification.Data.Repository
{
    public class MessageTemplateRepository : EfCoreBaseRepository<Guid, MessageTemplate>, IMessageTemplateRepository
    {
        public MessageTemplateRepository(NotificationContext dbContext) : base(dbContext)
        {
        }
    }
}
