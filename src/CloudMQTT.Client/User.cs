using System.Collections.Generic;
using Newtonsoft.Json;

namespace CloudMQTT.Client
{
    public class User : UserReference
    {
        [JsonProperty("acls")]
        public List<UserRule> Rules { get; set; } = new List<UserRule>();
    }
}
