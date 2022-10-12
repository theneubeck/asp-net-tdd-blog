using System;

namespace Blog.Tests.Models;

public class BlogPostModel
{
    public string Title { get; set; }
    public string Body { get; set; }
    public Guid Id { get; set; }
}
