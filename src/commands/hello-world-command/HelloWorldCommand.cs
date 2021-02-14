using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Mesi.Notify.Core;

namespace Mesi.Notify.Commands.HelloWorld
{
    [CommandName("hello-world.command")]
    public class HelloWorldCommand : IExecutableCommand
    {
        /// <inheritdoc />
        public IEnumerable<CommandProperty> RequiredPropertiesWithDefaultValues()
        {
            return Enumerable.Empty<CommandProperty>();
        }

        /// <inheritdoc />
        public IEnumerable<CommandProperty> OptionalPropertiesWithDefaultValues()
        {
            return new[] {new CommandProperty("name", "Your name")};
        }

        /// <inheritdoc />
        public Task<Result<CommandResponse>> Execute(IEnumerable<CommandProperty> propertiesEnumerable)
        {
            var properties = propertiesEnumerable.ToImmutableList();

            var nameProperty = properties.FirstOrDefault(p => p.Name == "name");

            if (nameProperty == null)
            {
                return Task.FromResult(Result.Failure<CommandResponse>("Unable to execute command with missing 'name' property."));
            }

            var name = nameProperty.Value;

            var response = string.IsNullOrWhiteSpace(name)
                ? new CommandResponse("Hello world")
                : new CommandResponse($"Hello {name}");

            return Task.FromResult(Result.Success(response));
        }
    }
}