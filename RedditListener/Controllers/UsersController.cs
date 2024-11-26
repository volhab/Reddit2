using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedditListener.Entities;
using RedditListener.Models;

namespace RedditListener.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly RedditContext _context;

        public UsersController(RedditContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("MostPosts")]
        public async Task<IEnumerable<User>> GetUsers()
        {
            // for simplicity will return 5 users with the most posts

            var usersPosts = await _context.Posts.Where(p => p.SubredditId == 1)
                .Join(_context.Users, 
                p => p.AuthorRedditId, 
                u => u.AuthorRedditId,
                (p, u) => new { p, u }).ToListAsync();


            var topVotes = usersPosts.GroupBy(x => x.p.AuthorRedditId)
                .Select(group => new User { Name = group.Select(g => g.u.Name).First(), Posts = group.Count()  })
                .OrderByDescending(x => x.Posts)
                .Take(5).ToList();

            return topVotes;
        }
    }
}
