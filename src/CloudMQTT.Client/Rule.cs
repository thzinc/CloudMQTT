using Newtonsoft.Json;

namespace CloudMQTT.Client
{
    public class Rule
    {
        [JsonProperty("type")]
        public RuleType RuleType { get; set; }
        public string User { get; set; }
        public string Pattern { get; set; }
        public bool Read { get; set; }
        public bool Write { get; set; }
    }
}
