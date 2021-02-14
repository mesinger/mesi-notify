using System.Collections.Generic;
using System.Threading.Tasks;
using Mesi.Notify.Core;

namespace Mesi.Notify.ApplicationLayer.Executions
{
    public interface IExecuteCommand
    {
        Task Execute(CommandName name, IEnumerable<CommandProperty> properties);
    }
    
    public interface IExecuteCommandWithPropertiesAsJson
    {
        Task Execute(CommandName name, string propertiesAsJson);
    }
}