using System.Threading.Tasks;
using Mesi.Notify.ApplicationLayer.Executions;
using Mesi.Notify.ApplicationLayer.Visuals;
using Mesi.Notify.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using web_app.Models;

namespace web_app.Pages
{
    public class CommandModel : PageModel
    {
        private readonly IGetCommandByName _getCommandByName;
        private readonly IExecuteCommandWithPropertiesAsJson _executeCommandWithPropertiesAsJson;

        public CommandModel(IGetCommandByName getCommandByName, IExecuteCommandWithPropertiesAsJson executeCommandWithPropertiesAsJson)
        {
            _getCommandByName = getCommandByName;
            _executeCommandWithPropertiesAsJson = executeCommandWithPropertiesAsJson;
        }
        
        public CommandViewModel Command { get; private set; }
        
        public IActionResult OnGet(string name)
        {
            var command = _getCommandByName.GetByName(new CommandName(name));

            if (command == null)
            {
                return NotFound();
            }

            Command = command.ToViewModel();
            
            return Page();
        }
    }
}