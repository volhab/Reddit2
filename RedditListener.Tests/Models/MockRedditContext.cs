using MockQueryable;
using MockQueryable.Moq;
using Moq;
using RedditListener.Entities;
using RedditListener.Models;

namespace RedditListener.Tests.Models
{
    public static class MockRedditContext
    {
        public static Mock<RedditContext> CreateMockDbContext()
        {
            var posts = new List<PostModel>
            {
                new PostModel { Id = 1, UpVotes = 5, CrossPosts = 1, SubredditId = 1, AuthorRedditId = "1", Title = "Post 1" },
                new PostModel { Id = 2, UpVotes = 2, CrossPosts = 2, SubredditId = 1, AuthorRedditId = "2", Title = "Post 2" },
                new PostModel { Id = 3, UpVotes = 4, CrossPosts = 3, SubredditId = 1, AuthorRedditId = "1", Title = "Post 3" },
                new PostModel { Id = 4, UpVotes = 4, CrossPosts = 4, SubredditId = 1, AuthorRedditId = "2", Title = "Post 4" },
                new PostModel { Id = 5, UpVotes = 1, CrossPosts = 5, SubredditId = 1, AuthorRedditId = "3", Title = "Post 5" },
                new PostModel { Id = 6, UpVotes = 3, CrossPosts = 100, SubredditId = 2, AuthorRedditId = "1", Title = "Post 6" },
                new PostModel { Id = 7, UpVotes = 6, CrossPosts = 10, SubredditId = 2, AuthorRedditId = "2", Title = "Post 7" },
                new PostModel { Id = 8, UpVotes = 5, CrossPosts = 1, SubredditId = 1, AuthorRedditId = "1", Title = "Post 8" },
            };
            var mockPosts = posts.AsQueryable().BuildMock();
            var mockPostsDbSet = posts.AsQueryable().BuildMockDbSet();

            var users = new List<UserModel>
            {
                new UserModel { Id = 1, AuthorRedditId = "1", Name = "User 1" },
                new UserModel { Id = 2, AuthorRedditId = "2", Name = "User 2" },
                new UserModel { Id = 3, AuthorRedditId = "3", Name = "User 3" },
                new UserModel { Id = 4, AuthorRedditId = "4", Name = "User 4" },
            };
            var mockUsers = users.AsQueryable().BuildMock();
            var mockUsersDbSet = users.AsQueryable().BuildMockDbSet();

            var subreddits = new List<SubredditModel>
            {
                new() {
                    Id = 1,
                    Subscribers = 55,
                    Name = "test"
                }
            };
            var mockSubreddits = subreddits.AsQueryable().BuildMock();
            var mockSubredditsDbSet = subreddits.AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<RedditContext>();
            mockContext.Setup(c => c.Posts).Returns(mockPostsDbSet.Object);
            mockContext.Setup(c => c.Users).Returns(mockUsersDbSet.Object);
            mockContext.Setup(c => c.Subreddits).Returns(mockSubredditsDbSet.Object);

            return mockContext;
        }
    }
}
