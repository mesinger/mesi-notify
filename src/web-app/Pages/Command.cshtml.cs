using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Mesi.Notify.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using web_app.Models;

namespace web_app.Pages
{
    public class CommandModel : PageModel
    {
        private readonly ICommandFactory _commandFactory;
        private readonly ICommandResponseSender _commandResponseSender;

        public CommandModel(ICommandFactory commandFactory, ICommandResponseSender commandResponseSender)
        {
            _commandFactory = commandFactory;
            _commandResponseSender = commandResponseSender;
        }
        
        public CommandViewModel Command { get; private set; }
        
        public IActionResult OnGet(string name)
        {
            var command = _commandFactory.GetByName(new CommandName(name));

            if (command == null)
            {
                return NotFound();
            }

            Command = command.ToViewModel();
            return Page();
        }

        public async Task<IActionResult> OnPost(string name)
        {
            var command = _commandFactory.GetExecutableByName(new CommandName(name));

            if (command == null)
            {
                return NotFound();
            }

            Command = command.ToViewModel();

            var (isSuccess, _, value) = await command.Execute(command.RequiredPropertiesWithDefaultValues());

            if (isSuccess)
            {
                await _commandResponseSender.SendCommandResponse(value,
                    new EmailRecipient("test@holobolo.at"));
            }
                    
            return Page();
        }
    }
}