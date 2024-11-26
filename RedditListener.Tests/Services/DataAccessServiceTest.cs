using Microsoft.Extensions.Options;
using Moq;
using RedditListener.Entities;
using RedditListener.Models;
using RedditListener.Services;
using RedditListener.Tests.Models;

namespace RedditListener.Tests.Services
{
    [TestClass]
    public class DataAccessServiceTest
    {
        Mock<RedditContext> TestRedditContext = new Mock<RedditContext>();
        Mock<IOptions<RedditSettings>> TestSettings = new Mock<IOptions<RedditSettings>>();

        [TestMethod]
        public async Task SavePosts()
        {
            // Arrange
            TestRedditContext = MockRedditContext.CreateMockDbContext();
            var service = new DataAccessService(TestRedditContext.Object, TestSettings.Object);
            var posts = new List<Post>
            {
                new Post 
                {  
                    created_utc = 1730399295.0, 
                    ups = 9, 
                    subreddit_subscribers = 999, 
                    num_crossposts = 50
                }};

            // Act
            await service.SavePosts(posts);

            //Assert
        }

        [TestMethod]
        public async Task SaveUsers()
        {
            // Arrange
            TestRedditContext = MockRedditContext.CreateMockDbContext();
            var service = new DataAccessService(TestRedditContext.Object, TestSettings.Object);
            var posts = new List<Post>
            {
                new Post
                {
                    ups = 9,
                    subreddit_subscribers = 999,
                    num_crossposts = 50,
                    created_utc = 1730399295.0,
                    author = "user",
                   author_fullname = "xxx" 
                }
            };

            // Act
            await service.SaveUsers(posts);

            //Assert
        }

        [TestMethod]
        public async Task UpdateSubreddit()
        {
            // Arrange
            TestRedditContext = MockRedditContext.CreateMockDbContext();
            var service = new DataAccessService(TestRedditContext.Object, TestSettings.Object);
            var post = new Post { created_utc = 10, ups = 9, subreddit_subscribers = 999, num_crossposts = 50 };

            // Act
            await service.UpdateSubreddit(post);

            //Assert
            var subreddit = TestRedditContext.Object.Subreddits.First(s => s.Id == 1);
            Assert.AreEqual(999, subreddit.Subscribers);
        }
    }
}