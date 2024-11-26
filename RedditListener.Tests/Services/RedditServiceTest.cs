using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using RedditListener.Entities;
using RedditListener.Services;
using System.Net;
using System.Text.Json;

namespace RedditListener.Tests.Services
{
    [TestClass]
    public class RedditServiceTest
    {
        Mock<IOptions<RedditSettings>> TestOptions = new Mock<IOptions<RedditSettings>>();
        Mock<HttpMessageHandler> TestHandler = new Mock<HttpMessageHandler>();

    [TestMethod]
        public async Task GetToken_ReturnsToken()
        {
            // Arrange
            var response = new AuthResponse
            {
                access_token = "token"
            };
            TestHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(response))
                });

            TestOptions.Setup(o => o.Value).Returns(new RedditSettings
            {
                AppId = "Value1",
                AppSecret = "Value2",
                Subreddit = "subreddit"
            });

            var httpClient = new HttpClient(TestHandler.Object);
            var service = new RedditService(TestOptions.Object, httpClient);

            // Act
            var result = await service.GetToken();

            // Assert
            Assert.AreEqual("token", result);
        }

        [TestMethod]
        public async Task ReadPosts_ReturnsPosts()
        {
            // Arrange
            var response = new PostResponse
            {
                responseCode = HttpStatusCode.OK,
                ratelimitRemaining = 100
            };
            TestHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(response))
                });

            TestOptions.Setup(o => o.Value).Returns(new RedditSettings
            {
                AppId = "Value1",
                AppSecret = "Value2",
                Subreddit = "subreddit"
            });

            var httpClient = new HttpClient(TestHandler.Object);
            var service = new RedditService(TestOptions.Object, httpClient);

            // Act
            var result = await service.ReadPosts("token", "after");

            // Assert
            Assert.AreEqual(100, result.ratelimitRemaining);
        }
    }
}