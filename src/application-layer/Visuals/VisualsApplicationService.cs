using System.Collections.Generic;
using System.Linq;
using Mesi.Notify.Core;

namespace Mesi.Notify.ApplicationLayer.Visuals
{
    public class VisualsApplicationService : IGetAvailableCommandNames, IGetCommandByName
    {
        private readonly ICommandRepository _commandRepository;
        private readonly ICommandFactory _commandFactory;

        public VisualsApplicationService(ICommandRepository commandRepository, ICommandFactory commandFactory)
        {
            _commandRepository = commandRepository;
            _commandFactory = commandFactory;
        }
        
        /// <inheritdoc />
        public IEnumerable<CommandName> GetAll()
        {
            return
                from command in _commandRepository.GetAllCommands()
                select command.GetCommandName();
        }

        /// <inheritdoc />
        public ICommand? GetByName(CommandName commandName)
        {
            return _commandFactory.GetByName(commandName);
        }
    }
}