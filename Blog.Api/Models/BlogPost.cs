using System.ComponentModel.DataAnnotations;

namespace Blog.Api.Models;

public class BlogPost
{
    [Required]
    [MinLength(10)]
    public string Body { get; set; }
    
    [Required]
    public string Title { get; set; }
    public Guid Id { get; set; }
    
}
