using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RedditListener.Entities;
using RedditListener.Interfaces;
using RedditListener.Models;

namespace RedditListener.Services
{
    public class DataAccessService: IDataAccessService
    {
        private readonly RedditContext _context;
        private readonly RedditSettings _settings;

        public DataAccessService(RedditContext context, IOptions<RedditSettings> settings)
        {
            _context = context;
            _settings = settings.Value;
        }

        public async Task SavePosts(List<Post> posts)
        {
            try
            {
                var postModels = posts.Select(p => new PostModel
                {
                    Id = Convert.ToInt64(p.created_utc.ToString()),
                    Title = p.title,
                    AuthorRedditId = p.author_fullname ?? "",
                    UpVotes = p.ups,
                    CrossPosts = p.num_crossposts,
                    SubredditId = 1
                }).ToList();

                await _context.Posts.AddRangeAsync(postModels);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task SaveUsers(List<Post> posts)
        {
            try
            {
                var userModels = posts.Select(p => new UserModel
                {
                    Id = Convert.ToInt64(p.created_utc.ToString()),
                    AuthorRedditId = p.author_fullname ?? "",
                    Name = p.author ?? "unknown"
                }).ToList();

                _context.Users.AddRange(userModels);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task UpdateSubreddit(Post post)
        {
            try
            {
                var sub = await _context.Subreddits.FirstOrDefaultAsync(s => s.Id == 1);
                if (sub != null) 
                {
                    sub.Subscribers = post.subreddit_subscribers;
                    _context.Subreddits.Update(sub);
                }
                else
                {
                    var newSub = new SubredditModel { Id = 1, Name = _settings.Subreddit, Subscribers = post.subreddit_subscribers  };
                    _context.Subreddits.Add(newSub);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
