namespace Blog.Api.Models
{
    public class BlogPost
    {
        public readonly string Type = "post";
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
