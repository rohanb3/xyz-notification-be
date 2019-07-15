using Mapster;
using Xyzies.Notification.API.Models;
using Xyzies.Notification.Services.Models;

namespace Xyzies.Notification.API.Mappers
{
    public class MapperConfigure
    {
        public static void Configure()
        {
            TypeAdapterConfig<EmailParametersModel, EmailParameters>.NewConfig();
        }
    }
}
