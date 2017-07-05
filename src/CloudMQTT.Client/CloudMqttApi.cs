using System;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestEase;

namespace CloudMQTT.Client
{
    public static class CloudMqttApi
    {
        /// <summary>
        /// Gets an instance of the CloudMQTT API client
        /// </summary>
        /// <param name="username">Username to authenticate to the API with</param>
        /// <param name="password">Password to authenticate to the API with</param>
        /// <param name="baseUri">Optional base URI to connect to</param>
        /// <returns>Instance of the CloudMQTT API client</returns>
        public static ICloudMqttApi GetInstance(string username, string password, string baseUri = "https://api.cloudmqtt.com")
        {
            var client = new RestClient(baseUri)
            {
                JsonSerializerSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy(),
                    },
                },
            }.For<ICloudMqttApi>();

            var value = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
            client.Authorization = new AuthenticationHeaderValue("Basic", value);

            return client;
        }
    }
}