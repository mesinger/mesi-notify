using System.Collections.Generic;
using Mesi.Notify.Core;

namespace Mesi.Notify.ApplicationLayer.Visuals
{
    public interface IGetAvailableCommandNames
    {
        IEnumerable<CommandName> GetAll();
    }
}