namespace Blog.Api.Models
{
    public class BlogPost
    {
        public Guid Id { get; set; }
        
        public string Type { get;  } = "post";
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
