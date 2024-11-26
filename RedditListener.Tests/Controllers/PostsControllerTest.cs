using Moq;
using RedditListener.Controllers;
using RedditListener.Models;
using RedditListener.Tests.Models;

namespace RedditListener.Tests.Controllers
{
    [TestClass]
    public class PostControllerTest
    {
        Mock<RedditContext> TestRedditContext = new Mock<RedditContext>();

        [TestMethod]
        public async Task GetPosts_ReturnsFiveWithMostUps()
        {
            // Arrange
            TestRedditContext = MockRedditContext.CreateMockDbContext();

            var controller = new PostsController(TestRedditContext.Object);

            // Act
            var result = await controller.GetPosts();

            // Assert
            Assert.AreEqual(5, result.Count());
            Assert.AreEqual(5, result.First().UpVotes);
            Assert.AreEqual("Post 1", result.First().Title);
        }

        [TestMethod]
        public async Task GetCrossPosts_ReturnsFiveWithMostUps()
        {
            // Arrange
            TestRedditContext = MockRedditContext.CreateMockDbContext();

            var controller = new PostsController(TestRedditContext.Object);

            // Act
            var result = await controller.GetCrossPosts();

            // Assert
            Assert.AreEqual(5, result.Count());
            Assert.AreEqual(5, result.First().CrossPosts);
            Assert.AreEqual("Post 5", result.First().Title);
        }
    }
}