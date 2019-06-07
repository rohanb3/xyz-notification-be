using System;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Xyzies.Notification.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : BaseController
    {
        private readonly ILogger<ValuesController> _logger = null;

        /// <summary>
        /// Value constructor
        /// </summary>
        /// <param name="logger"></param>
        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Notification Api" })]
        public async Task<IActionResult> SendEmail()
        {
            
            return Ok(new string[] { "value1", "value2" });
        }
    }
}
