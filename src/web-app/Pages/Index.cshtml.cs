using System.Collections.Generic;
using System.Linq;
using Mesi.Notify.ApplicationLayer.Visuals;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace web_app.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IGetAvailableCommandNames _getAvailableCommandNames;

        public IndexModel(IGetAvailableCommandNames getAvailableCommandNames)
        {
            _getAvailableCommandNames = getAvailableCommandNames;
        }

        public IEnumerable<string> AvailableCommandNames { get; private set; }

        public void OnGet()
        {
            AvailableCommandNames = from name in _getAvailableCommandNames.GetAll() select name.Name;
        }
    }
}