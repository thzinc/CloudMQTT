namespace CloudMQTT.Client
{
    public class Rule
    {
        public string Username { get; set; }
        public string Topic { get; set; }
        public bool Read { get; set; }
        public bool Write { get; set; }

    }
}
