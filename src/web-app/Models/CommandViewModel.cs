using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace web_app.Models
{
    public record CommandViewModel(string Name, IEnumerable<CommandPropertyViewModel> Required,
        IEnumerable<CommandPropertyViewModel> Optional)
    {
        public string RequiredPropertiesToJson()
        {
            return JsonConvert.SerializeObject(Required);
        }
        
        public string OptionalPropertiesToJson()
        {
            return JsonConvert.SerializeObject(Optional);
        }
    }

    public record CommandPropertyViewModel([property: JsonProperty("name")] string Name, [property: JsonProperty("value")]string Value);
}