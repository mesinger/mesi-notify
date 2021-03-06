using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Mesi.Notify.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Mesi.Notify.ApplicationLayer.Executions
{
    public class ExecutionsApplicationService : IExecuteCommand, IExecuteCommandWithPropertiesAsJson
    {
        private readonly ICommandFactory _commandFactory;
        private readonly ICommandResponseSender _commandResponseSender;
        private readonly ILogger<ExecutionsApplicationService> _logger;

        public ExecutionsApplicationService(ICommandFactory commandFactory,
            ICommandResponseSender commandResponseSender, ILogger<ExecutionsApplicationService> logger)
        {
            _commandFactory = commandFactory;
            _commandResponseSender = commandResponseSender;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<Result> Execute(CommandName name, IEnumerable<CommandProperty> properties, IRecipient recipient)
        {
            var command = _commandFactory.GetExecutableByName(name);

            if (command == null)
            {
                _logger.LogWarning("Trying to execute unknown command '{}'", name.Name);
                return Result.Failure("Unknown command");
            }

            var result = await command.Execute(properties);

            if (result.IsSuccess)
            {
                await _commandResponseSender.SendCommandResponse(result.Value, recipient);
                return Result.Success();
            }

            return Result.Failure(result.Error);
        }

        /// <inheritdoc />
        public async Task<Result> Execute(CommandName name, string propertiesAsJson, IRecipient recipient)
        {
            if (string.IsNullOrWhiteSpace(propertiesAsJson))
            {
                return Result.Failure("Invalid properties");
            }
            
            return await Execute(name, ParsePropertiesFromJson(propertiesAsJson), recipient);
        }

        private IEnumerable<CommandProperty> ParsePropertiesFromJson(string propertiesJson)
        {
            try
            {
                return
                    JObject.Parse(propertiesJson)
                        .Children().Cast<JProperty>()
                        .ToDictionary(x => x.Name, x => (string) x.Value!)
                        .Select((kv, _) => new CommandProperty(kv.Key, kv.Value));
            }
            catch (Exception)
            {
                _logger.LogWarning("Unable to parse properties for command");
                return Enumerable.Empty<CommandProperty>();
            }
        }
    }
}