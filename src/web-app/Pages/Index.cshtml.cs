using System.Collections.Generic;
using System.Linq;
using Mesi.Notify.Core;
using Microsoft.AspNetCore.Mvc.RazorPages;
using web_app.Models;

namespace web_app.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICommandFactory _commandFactory;

        public IndexModel(ICommandFactory commandFactory)
        {
            _commandFactory = commandFactory;
        }

        public IEnumerable<CommandViewModel> Commands { get; private set; }

        public void OnGet()
        {
            var commands = _commandFactory.GetAllCommands();
            Commands = commands.Select(command => command.ToViewModel());
        }
    }
}