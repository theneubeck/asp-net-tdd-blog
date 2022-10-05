using System.ComponentModel.DataAnnotations;

namespace Blog.Api.Models
{
    public class BlogPost
    {
        public Guid Id { get; set; }
        
        public string Type { get;  } = "post";
        [Required]
        public string Title { get; set; }
        [Required, StringLength(10, ErrorMessage = "Name length can't be more than 10.")]
        public string Body { get; set; }
    }
}
