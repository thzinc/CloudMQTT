using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace CloudMQTT.Client
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum RuleType
    {
        [EnumMember(Value = "topic")]
        Topic,

        [EnumMember(Value = "pattern")]
        Pattern
    }
}
