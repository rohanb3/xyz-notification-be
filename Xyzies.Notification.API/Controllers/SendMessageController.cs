using System;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Xyzies.Notification.Services.Common.Interfaces;
using Xyzies.Notification.Services.Models;

namespace Xyzies.Notification.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendMessage : BaseController
    {
        private readonly ILogger<SendMessage> _logger = null;
        private readonly IMailerService _mailer = null;

        /// <summary>
        /// Value constructor
        /// </summary>
        /// <param name="logger"></param>
        public SendMessage(ILogger<SendMessage> logger, IMailerService mailer)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            _mailer = mailer ??
                throw new ArgumentNullException(nameof(mailer));
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Notification Api" })]
        public async Task<IActionResult> SendEmail()
        {
            try
            {
                await _mailer.SendMail(new EmailParametersModel());
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
