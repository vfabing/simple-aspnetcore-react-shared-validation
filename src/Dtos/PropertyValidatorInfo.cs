using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace simple_aspnetcore_react_shared_validation.Dtos
{

    public class PropertyValidatorInfo
    {
        public string Name { get; set; }

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalData { get; set; } = new Dictionary<string, object>();

        public string ErrorMessage { get; set; }
    }
}
