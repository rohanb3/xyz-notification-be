using System;
using Xyzies.Notification.Data.Entity;
using Xyzies.Notification.Data.Repository.Behaviour;

namespace Xyzies.Notification.Data.Repository
{
    public class LogRepository : EfCoreBaseRepository<Guid, Log>, ILogRepository
    {
        public LogRepository(NotificationContext dbContext) : base(dbContext)
        {
        }
    }
}
