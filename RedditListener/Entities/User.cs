using System.ComponentModel.DataAnnotations;

namespace RedditListener.Entities
{
    public class User
    {
        [Key]
        public string Name { get; set; }
        public int Posts { get; set; }
    }
}
