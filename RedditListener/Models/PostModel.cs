namespace RedditListener.Entities
{
    public class PostModel
    {
        public long Id { get; set; }
        public int SubredditId { get; set; }
        public string Title { get; set; }
        public int UpVotes { get; set; }
        public int CrossPosts{ get; set; }
        public string AuthorRedditId { get; set; }
    }
}
