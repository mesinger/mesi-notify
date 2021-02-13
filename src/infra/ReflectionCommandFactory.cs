using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mesi.Notify.Core;

namespace Mesi.Notify.Infra
{
    public class ReflectionCommandFactory : ICommandFactory
    {
        private readonly IFileSystem _fileSystem;

        public ReflectionCommandFactory(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        /// <inheritdoc />
        public IExecutableCommand? GetExecutableByName(CommandName name)
        {
            var commandType =
                _fileSystem
                    .Ls(_fileSystem.GetExecutingBaseDirectory())
                    .Where(file => file.EndsWith(".dll"))
                    .Select(Assembly.LoadFile)
                    .SelectMany(assembly => assembly.GetTypes())
                    .Where(type => type.GetInterfaces().Contains(typeof(ICommand)))
                    .FirstOrDefault(type =>
                        (type.GetCustomAttribute(typeof(CommandNameAttribute)) as CommandNameAttribute)?.CommandName ==
                        name.Name);

            try
            {
                return commandType?.GetConstructor(Type.EmptyTypes)?.Invoke(Array.Empty<object>()) is IExecutableCommand command
                    ? command
                    : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <inheritdoc />
        public ICommand? GetByName(CommandName name)
        {
            return GetExecutableByName(name);
        }

        /// <inheritdoc />
        public IEnumerable<ICommand> GetAllCommands()
        {
            return 
                _fileSystem.Ls(_fileSystem.GetExecutingBaseDirectory())
                    .Where(file => file.EndsWith(".dll"))
                    .Select(Assembly.LoadFile)
                    .SelectMany(assembly => assembly.GetTypes())
                    .Where(type => type.GetInterfaces().Contains(typeof(IExecutableCommand)))
                    .Select(type =>
                    {
                        try
                        {
                            return type.GetConstructor(Type.EmptyTypes)?.Invoke(Array.Empty<object>()) as ICommand;
                        }
                        catch (Exception)
                        {
                            return null;
                        }
                    })
                    .Where(command => command != null)!;
        }
    }
}