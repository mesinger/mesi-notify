using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Mesi.Notify.Core;

namespace Mesi.Notify.Commands.Addition
{
    [CommandName("addition.command")]
    public class AdditionCommand : IExecutableCommand
    {
        /// <inheritdoc />
        public IEnumerable<CommandProperty> RequiredPropertiesWithDefaultValues()
        {
            return new[] {new CommandProperty("a", "1"), new CommandProperty("b", "2")};
        }

        /// <inheritdoc />
        public IEnumerable<CommandProperty> OptionalPropertiesWithDefaultValues()
        {
            return new[] {new CommandProperty("c", "3")};
        }

        /// <inheritdoc />
        public Task<Result<CommandResponse>> Execute(IEnumerable<CommandProperty> propertiesEnumerable)
        {
            var properties = propertiesEnumerable.ToImmutableList();

            var aString = properties.FirstOrDefault(property => property.Name == "a")?.Value ?? string.Empty;
            var bString = properties.FirstOrDefault(property => property.Name == "b")?.Value ?? string.Empty;
            var cString = properties.FirstOrDefault(property => property.Name == "c")?.Value ?? string.Empty;

            if (!int.TryParse(aString, out var a) || !int.TryParse(bString, out var b))
            {
                return Task.FromResult(Result.Failure<CommandResponse>("Unable to calculate without a or b"));
            }

            if (!int.TryParse(cString, out var c))
            {
                return Task.FromResult(Result.Success(new CommandResponse($"Result = {a + b}")));
            }
            
            return Task.FromResult(Result.Success(new CommandResponse($"Result = {a + b + c}")));
        }
    }
}