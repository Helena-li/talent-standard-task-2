using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Common.Commands;
using Microsoft.AspNetCore.Mvc;
using RawRabbit;

namespace Talent.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProfileController : Controller
    {
        private readonly IBusClient _busClient;
        public ProfileController(IBusClient busClient)
        {
            _busClient = busClient;
        }

        // GET: /<controller>/
        [HttpGet("")]
        public IActionResult Get()
        {
            return Content("Hello from Profile Api");
        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]AuthenticateUser command)
        {
            await _busClient.PublishAsync(command);

            return Accepted($"userProfile/{command.Email}");
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody]CreateUser command)
        {
            try
            {
                await _busClient.PublishAsync(command);

                return Accepted($"createUser/{command.Email}");
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }

        }
    }
}
