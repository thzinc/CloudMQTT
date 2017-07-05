namespace CloudMQTT.Client
{
    public class RuleReference
    {
        public string Username { get; set; }
        public string Topic { get; set; }

        public static implicit operator RuleReference(Rule rule) => new RuleReference()
        {
            Topic = rule.Topic,
            Username = rule.Username,
        };
    }
}
