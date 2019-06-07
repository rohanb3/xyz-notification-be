using SendGrid;
using System.Threading.Tasks;
using Xyzies.Notification.Services.Models;

namespace Xyzies.Notification.Services.Common.Interfaces
{
    public interface IMailerService
    {
        Task<Response> SendMail(EmailParametersModel model);
    }
}
