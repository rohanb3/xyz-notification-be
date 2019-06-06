using System;
using Xyzies.Notification.Data.Entity;
using Xyzies.Notification.Data.Repository.Behaviour;

namespace Xyzies.Notification.Data.Repository
{
    public class EmailTemplateRepository : EfCoreBaseRepository<Guid, EmailTemplate>, IEmailTemplateRepository
    {
        public EmailTemplateRepository(NotificationContext dbContext) : base(dbContext)
        {
        }
    }
}
