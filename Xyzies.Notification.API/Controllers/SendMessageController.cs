using System;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Xyzies.Notification.Services.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Xyzies.Notification.API.Models;
using Mapster;
using Xyzies.Notification.Services.Models;

namespace Xyzies.Notification.API.Controllers
{
    [Route("notification")]
    [ApiController]
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
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost("sendEmail/{token}/trusted")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK  /* 200 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden /* 403 */)]
        [SwaggerOperation(Tags = new[] { "Notification Api" })]
        public async Task<IActionResult> SendEmail([FromRoute]string token, [FromBody]EmailParametersModel model)
        {
            if (token != Consts.StaticToken)
            {
                return new ContentResult { StatusCode = 403 };
            }

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
            catch (NullReferenceException ex)
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
