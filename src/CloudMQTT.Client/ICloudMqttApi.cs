using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using RestEase;

namespace CloudMQTT.Client
{
    public interface ICloudMqttApi
    {
        [Header("Authorization")]
        AuthenticationHeaderValue Authorization { get; set; }

        [Get("user")]
        Task<List<UserReference>> GetUsers();

        [Get("user/{username}")]
        Task<User> GetUser([Path]string username);

        [Post("user")]
        [Header("Content-Type", "application/json")]
        Task<HttpResponseMessage> CreateUser([Body]NewUser user);

        [Delete("user/{username}")]
        Task<HttpResponseMessage> DeleteUser([Path]string username);

        [Get("acl")]
        Task<List<Rule>> GetRules();

        [Post("acl")]
        [Header("Content-Type", "application/json")]
        Task<HttpResponseMessage> CreateRule([Body]Rule rule);

        [Delete("acl")]
        [Header("Content-Type", "application/json")]
        Task DeleteRule([Body]RuleReference ruleReference);
    }
}
