namespace RedditListener.Entities
{
    public class Post
    {
        public string name { get; set; }
        public string title { get; set; }
        public int ups { get; set; }
        public int downs { get; set; }
        public long subreddit_subscribers { get; set; }
        public double created_utc { get; set; }
        public string? author_fullname { get; set; }
        public string? author { get; set; }
        public int posts_count { get; set; }
        public int num_crossposts { get; set; }
    }
}
