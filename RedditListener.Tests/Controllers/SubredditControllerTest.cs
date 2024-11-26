using Moq;
using RedditListener.Controllers;
using RedditListener.Models;
using RedditListener.Tests.Models;

namespace RedditListener.Tests.Controllers
{
    [TestClass]
    public class SubredditControllerTest
    {
        Mock<RedditContext> TestRedditContext = new Mock<RedditContext>();

        [TestMethod]
        public void GetSubscribers_ReturnsNumberOfSubscribers()
        {
            // Arrange
            TestRedditContext = MockRedditContext.CreateMockDbContext();

            var controller = new SubredditController(TestRedditContext.Object);

            // Act
            var result = controller.GetSubscribers();

            // Assert
            Assert.AreEqual(55, result);
        }
    }
}