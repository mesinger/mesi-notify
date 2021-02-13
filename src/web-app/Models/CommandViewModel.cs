using System.Collections.Generic;

namespace web_app.Models
{
    public record CommandViewModel(string Name, IEnumerable<CommandPropertyViewModel> Required, IEnumerable<CommandPropertyViewModel> Optional);

    public record CommandPropertyViewModel(string Name, string Value);
}