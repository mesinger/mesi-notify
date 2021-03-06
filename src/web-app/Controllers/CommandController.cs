using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Mesi.Notify.ApplicationLayer.Executions;
using Mesi.Notify.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace web_app.Controllers
{
    [Route("api/command")]
    [Authorize]
    public class CommandController : Controller
    {
        private readonly IExecuteCommandWithPropertiesAsJson _executeCommandWithPropertiesAsJson;

        public CommandController(IExecuteCommandWithPropertiesAsJson executeCommandWithPropertiesAsJson)
        {
            _executeCommandWithPropertiesAsJson = executeCommandWithPropertiesAsJson;
        }
        
        [Route("execute")]
        [HttpPost]
        public async Task<IActionResult> SendCommand(string commandName, string properties)
        {
            var email = User.FindFirstValue("email");

            if (string.IsNullOrWhiteSpace(email))
            {
                return Unauthorized();
            }
            
            var result = await _executeCommandWithPropertiesAsJson.Execute(new CommandName(commandName), properties, new EmailRecipient(email));
            
            return result.IsSuccess
                ? Ok()
                : BadRequest(result.Error);
        }
    }
}