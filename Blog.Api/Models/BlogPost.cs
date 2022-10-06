using System.ComponentModel.DataAnnotations;

namespace Blog.Api.Models
{
    public class BlogPost
    {
        public Guid Id { get; set; }

        public string Type { get; } = "post";

        [Required] public string Title { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Body must be at least 10 chars")]
        public string Body { get; set; }
    }
}
