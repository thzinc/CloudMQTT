namespace CloudMQTT.Client
{
    public class UserRule
    {
        public string Topic { get; set; }
        public bool Read { get; set; }
        public bool Write { get; set; }

        public static implicit operator UserRule(Rule rule) => new UserRule
        {
            Topic = rule.Topic,
            Read = rule.Read,
            Write = rule.Write,
        };
    }
}