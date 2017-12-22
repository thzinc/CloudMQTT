using System;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestEase;
using Xunit;

namespace CloudMQTT.Client.Tests
{
    public class WhenGettingUsers
    {
        private readonly ICloudMqttApi _subject;
        public WhenGettingUsers()
        {
            var username = Environment.GetEnvironmentVariable("CLOUDMQTT_USER");
            var password = Environment.GetEnvironmentVariable("CLOUDMQTT_PASSWORD");
            _subject = CloudMqttApi.GetInstance(username, password);
        }

        [Fact]
        public async Task ItShouldCreateAndDeleteUserSuccessfully()
        {
            var expectedUser = new NewUser
            {
                Password = $"{Guid.NewGuid()}",
                Username = $"test-{Guid.NewGuid()}",
            };

            var expectedRule = new Rule
            {
                RuleType = RuleType.Topic,
                Read = true,
                Pattern = "test",
                User = expectedUser.Username,
                Write = true,
            };

            var createUserResponse = await _subject.CreateUser(expectedUser);
            createUserResponse.IsSuccessStatusCode.Should().BeTrue();

            var createRuleResponse = await _subject.CreateRule(expectedRule);
            createRuleResponse.IsSuccessStatusCode.Should().BeTrue();

            var actual = await _subject.GetUser(expectedUser.Username);
            actual.Should().NotBeNull();
            actual.Username.Should().Be(expectedUser.Username);
            actual.Rules.Should().HaveCount(1);
            actual.Rules.Single().ShouldBeEquivalentTo(expectedRule);

            var users = await _subject.GetUsers();
            users.Should().Contain(u => u.Username == expectedUser.Username);

            var deleteResponse = await _subject.DeleteUser(expectedUser.Username);
            deleteResponse.IsSuccessStatusCode.Should().BeTrue();

            Func<Task> verifyUser = async () => await _subject.GetUser(expectedUser.Username);
            verifyUser.ShouldThrow<ApiException>()
                .And.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
