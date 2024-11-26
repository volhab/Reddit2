using Microsoft.AspNetCore.Mvc;
using RedditListener.Models;

namespace RedditListener.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubredditController : ControllerBase
    {
        private readonly RedditContext _context;

        public SubredditController(RedditContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Subscribers")]
        public long GetSubscribers()
        {
            return _context.Subreddits.FirstOrDefault(p => p.Id == 1)?.Subscribers ?? 0;
        }
    }
}
