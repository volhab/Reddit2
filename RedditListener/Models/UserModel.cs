using System.ComponentModel.DataAnnotations;

namespace RedditListener.Models
{
    public class UserModel
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string AuthorRedditId { get; set; }
    }
}
