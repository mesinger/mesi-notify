using System.Threading.Tasks;
using Mesi.Notify.ApplicationLayer.Executions;
using Mesi.Notify.Core;
using Microsoft.AspNetCore.Mvc;

namespace web_app.Controllers
{
    [Route("api/command")]
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
            var result = await _executeCommandWithPropertiesAsJson.Execute(new CommandName(commandName), properties);
            
            return result.IsSuccess
                ? Ok()
                : BadRequest(result.Error);
        }
    }
}