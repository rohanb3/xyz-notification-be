using System;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Xyzies.Notification.Services.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using IdentityServiceClient.Filters;
using IdentityServiceClient;
using Microsoft.AspNetCore.Http;
using Xyzies.Notification.API.Models;
using Mapster;
using Xyzies.Notification.Services.Models;

namespace Xyzies.Notification.API.Controllers
{
    [Route("notification")]
    [ApiController]
    [Authorize]
    public class SendMessage : BaseController
    {
        private readonly ILogger<SendMessage> _logger = null;
        private readonly IMailerService _mailer = null;

        /// <summary>
        /// SendMessage constructor
        /// </summary>
        /// <param name="logger"></param>
        public SendMessage(ILogger<SendMessage> logger, IMailerService mailer)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            _mailer = mailer ??
                throw new ArgumentNullException(nameof(mailer));
        }

        /// <summary>
        /// Send Email
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("sendEmail")]
        [AccessFilter(Const.Permissions.NotificationEmail.PermissionForCreate)]
        [ProducesResponseType(typeof(void), StatusCodes.Status201Created  /* 201 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden /* 403 */)]
        [SwaggerOperation(Tags = new[] { "Notification Api" })]
        public async Task<IActionResult> SendEmail([FromBody]EmailParametersModel model)
        {
            try
            {
                var emailParams = model.Adapt<EmailParameters>();

                await _mailer.SendMail(emailParams);
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

        }
    }
}
