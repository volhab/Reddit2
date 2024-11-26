using RedditListener.Entities;

namespace RedditListener.Interfaces
{
    public interface IDataAccessService 
    {
        Task SavePosts(List<Post> posts);
        Task SaveUsers(List<Post> posts);
        Task UpdateSubreddit(Post post);
    }
}
