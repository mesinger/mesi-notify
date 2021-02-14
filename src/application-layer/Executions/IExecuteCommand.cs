using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Mesi.Notify.Core;

namespace Mesi.Notify.ApplicationLayer.Executions
{
    public interface IExecuteCommand
    {
        Task<Result> Execute(CommandName name, IEnumerable<CommandProperty> properties);
    }
    
    public interface IExecuteCommandWithPropertiesAsJson
    {
        Task<Result> Execute(CommandName name, string propertiesAsJson);
    }
}