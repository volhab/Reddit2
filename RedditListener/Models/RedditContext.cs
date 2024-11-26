using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RedditListener.Entities;

namespace RedditListener.Models
{
    public class RedditContext : DbContext
    {
        private readonly RedditSettings _settings;

        public RedditContext(DbContextOptions<RedditContext> options, IOptions<RedditSettings> settings)
    : base(options)
        {
            _settings = settings.Value;
        }

        public RedditContext() : base()
        {
        }

        public virtual DbSet<SubredditModel> Subreddits { get; set; } = null!;
        public virtual DbSet<UserModel> Users { get; set; } = null!;
        public virtual DbSet<PostModel> Posts { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("RedditDatabase");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubredditModel>().HasData(
                new SubredditModel { Id = 1, Name = _settings.Subreddit }
            );
        }
    }
}
