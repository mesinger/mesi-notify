using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace Mesi.Notify.Core
{
    public interface ICommand
    {
        CommandName GetCommandName()
        {
            var name = (this.GetType().GetCustomAttribute(typeof(CommandNameAttribute)) as CommandNameAttribute)
                ?.CommandName ?? "unknown.command";

            return new(name);
        }

        IEnumerable<CommandProperty> RequiredPropertiesWithDefaultValues();

        IEnumerable<CommandProperty> OptionalPropertiesWithDefaultValues();
    }

    public interface IExecutableCommand : ICommand
    {
        Task<Result<CommandResponse>> Execute(IEnumerable<CommandProperty> properties);
    }

    public record CommandName(string Name);

    public record CommandProperty(string Name, string Value);

    public record CommandResponse(string Data);

    public interface ICommandFactory
    {
        IExecutableCommand? GetExecutableByName(CommandName name);
        ICommand? GetByName(CommandName name);
    }

    public interface ICommandRepository
    {
        IEnumerable<ICommand> GetAllCommands();
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class CommandNameAttribute : Attribute
    {
        public CommandNameAttribute(string commandName)
        {
            CommandName = commandName;
        }

        public string CommandName { get; }
    }
}