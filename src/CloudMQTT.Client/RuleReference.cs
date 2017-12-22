using Newtonsoft.Json;

namespace CloudMQTT.Client
{
    public class RuleReference
    {
        [JsonProperty("type")]
        public RuleType RuleType { get; set; }
        public string User { get; set; }
        public string Pattern { get; set; }

        public static implicit operator RuleReference(Rule rule) => new RuleReference()
        {
            RuleType = rule.RuleType,
            Pattern = rule.Pattern,
            User = rule.User,
        };
    }
}
