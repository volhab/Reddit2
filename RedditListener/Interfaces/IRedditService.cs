using RedditListener.Entities;

namespace RedditListener.Interfaces
{
    public interface IRedditService
    {
        Task<string> GetToken();
        Task<PostResponse?> ReadPosts(string token, string after);
    }
}
