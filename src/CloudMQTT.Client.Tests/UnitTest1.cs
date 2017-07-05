using System;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using RestEase;
using Xunit;

namespace CloudMQTT.Client.Tests
{
    public class WhenGettingUsers
    {
        private readonly ICloudMqttApi _subject;
        public WhenGettingUsers()
        {
            _subject = RestClient.For<ICloudMqttApi>("https://api.cloudmqtt.com");
            var value = Convert.ToBase64String(Encoding.ASCII.GetBytes("user:pass"));
            _subject.Authorization = new AuthenticationHeaderValue("Basic", value);
        }

        [Fact]
        public async Task ItShouldCreateAndDeleteUserSuccessfully()
        {
            var expected = new NewUser
            {
                Password = $"{Guid.NewGuid()}",
                Username = $"test-{Guid.NewGuid()}",
            };
            await _subject.CreateUser(expected);

            var actual = await _subject.GetUser(expected.Username);
            actual.Should().NotBeNull();
            actual.Username.Should().Be(expected.Username);
            actual.Rules.Should().BeEmpty();
            
            await _subject.DeleteUser(expected.Username);

            await _subject.GetUser(expected.Username);
            // Action verifyUser = async () => await _subject.GetUser(expected.Username);
            // verifyUser();
            // verifyUser.ShouldThrow<Exception>();
        }
    }
}
