using System.Net;

namespace RedditListener.Entities
{
    public class PostResponse
    {
        public HttpStatusCode responseCode { get; set; }
        public int ratelimitUsed { get; set; }
        public double ratelimitRemaining { get; set; }
        public int ratelimitReset { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public string after { get; set; }
        public string before { get; set; }
        public int dist { get; set; }
        public List<Child> children { get; set; }
    }

    public class Child
    {
        public Post data { get; set; }
    }
}
