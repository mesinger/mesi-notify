using Mesi.Notify.Core;

namespace Mesi.Notify.ApplicationLayer.Visuals
{
    public interface IGetCommandByName
    {
        ICommand? GetByName(CommandName commandName);
    }
}