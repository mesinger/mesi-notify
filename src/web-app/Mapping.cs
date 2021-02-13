using System.Linq;
using Mesi.Notify.Core;
using web_app.Models;

namespace web_app
{
    public static class MappingConfig
    {
        public static CommandPropertyViewModel ToViewModel(this CommandProperty property) =>
            new(property.Name, property.Value);

        public static CommandViewModel ToViewModel(this ICommand command) => new(command.GetCommandName().Name,
            command.RequiredPropertiesWithDefaultValues().Select(prop => prop.ToViewModel()),
            command.OptionalPropertiesWithDefaultValues().Select(prop => prop.ToViewModel()));
    }
}