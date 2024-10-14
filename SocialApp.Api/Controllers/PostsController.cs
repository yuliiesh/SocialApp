using Microsoft.AspNetCore.Mvc;
using SocialApp.Api.Posts;
using SocialApp.Api.Posts.Create;
using SocialApp.Common.Posts.Get;
using SocialApp.Data.Models;
using SocialApp.Data.Repositories;

namespace SocialApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository _postRepository;

    public PostsController(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetPosts(CancellationToken cancellationToken)
    {
        var posts = await _postRepository.GetAll(cancellationToken);

        return Ok(new GetPostsResponse
        {
            Posts = posts.Select(x => x.ToDto()).ToList()
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request, CancellationToken cancellationToken)
    {
        var createdPost = await _postRepository.Save(request.ToPostModel(), cancellationToken);

        return Ok(createdPost.ToCreatePostResponse());
    }
}