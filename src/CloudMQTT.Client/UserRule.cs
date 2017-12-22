using Newtonsoft.Json;

namespace CloudMQTT.Client
{
    public class UserRule
    {
        [JsonProperty("topic")]
        public string Pattern { get; set; }
        public bool Read { get; set; }
        public bool Write { get; set; }

        public static implicit operator UserRule(Rule rule) => new UserRule
        {
            Pattern = rule.Pattern,
            Read = rule.Read,
            Write = rule.Write,
        };
    }
}