using System.Collections.Generic;

namespace CloudMQTT.Client
{
    public class User : UserReference
    {
        public List<Rule> Rules { get; set; } = new List<Rule>();
    }
}
