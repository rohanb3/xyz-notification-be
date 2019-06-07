using Microsoft.EntityFrameworkCore;
using Xyzies.Notification.Data.Entity;

namespace Xyzies.Notification.Data
{
    public class NotificationContext: DbContext
    {
        public NotificationContext(DbContextOptions<NotificationContext> options) : base(options)
        {
        }

        #region Entities

        public DbSet<TypeOfMessage> TypeOfMessages { get; set; }

        public DbSet<MessageTemplate> MessageTemplates { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
