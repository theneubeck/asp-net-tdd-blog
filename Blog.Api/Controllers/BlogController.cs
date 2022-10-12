using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Route("[controller]")]
public class BlogController : ControllerBase
{
}

