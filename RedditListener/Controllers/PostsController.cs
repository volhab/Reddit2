using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedditListener.Entities;
using RedditListener.Models;

namespace RedditListener.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly RedditContext _context;

        public PostsController(RedditContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("MostUpVotes")]
        public async Task<IEnumerable<PostModel>> GetPosts()
        {
            // for simplicity will return 5 posts with most up votes 

            var posts = await _context.Posts.Where(p => p.SubredditId == 1)
                .OrderByDescending(p => p.UpVotes).Take(5).ToListAsync();
            return posts;
        }

        [HttpGet]
        [Route("MostCrossPosts")]
        public async Task<IEnumerable<PostModel>> GetCrossPosts()
        {

            var posts = await _context.Posts.Where(p => p.SubredditId == 1)
                .OrderByDescending(p => p.CrossPosts).Take(5).ToListAsync();
            return posts;
        }
    }
}
