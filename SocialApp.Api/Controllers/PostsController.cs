using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialApp.Common.Posts;
using SocialApp.Common.Posts.Create;
using SocialApp.Common.Posts.Get;
using SocialApp.Data.Models;
using SocialApp.Data.Repositories;

namespace SocialApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IPostRepository _postRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IPostHandler _postHandler;

    public PostsController(
        IPostRepository postRepository,
        UserManager<IdentityUser> userManager,
        ICommentRepository commentRepository,
        IPostHandler postHandler)
    {
        _postRepository = postRepository;
        _userManager = userManager;
        _commentRepository = commentRepository;
        _postHandler = postHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetPosts()
    {
        var posts = await _postHandler.GetPosts(HttpContext.RequestAborted);

        return Ok(new GetPostsResponse
        {
            Posts = posts
        });
    }

    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetPosts([FromRoute] Guid userId)
    {
        var posts = await _postHandler.GetPosts(userId, HttpContext.RequestAborted);

        return Ok(new GetPostsResponse
        {
            Posts = posts
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
    {
        var response = await _postHandler.CreatePost(request, HttpContext.RequestAborted);

        return Ok(response);
    }

    [HttpDelete("{postId:guid}")]
    public async Task<IActionResult> DeletePost([FromRoute] Guid postId)
    {
        await _postHandler.DeletePost(postId, HttpContext.RequestAborted);
        return NoContent();
    }

    [HttpGet("fake")]
    public async Task<IActionResult> GenerateFakeData()
    {
        var userFaker = new Faker<IdentityUser>()
            .RuleFor(x => x.UserName, f => f.Person.UserName)
            .RuleFor(x => x.Email, f => f.Person.Email);

        var users = userFaker.Generate(10);

        var createdUsers = new List<IdentityUser>();
        foreach (var user in users)
        {
            await _userManager.CreateAsync(user, "Q5ryi78o1!");
            createdUsers.Add(await _userManager.FindByEmailAsync(user.Email));
        }

        var postFaker = new Faker<PostModel>()
            .RuleFor(x => x.Id, f => f.Random.Guid())
            .RuleFor(x => x.Title, f => f.Random.Words(3))
            .RuleFor(x => x.Content, f => f.Random.Words(100))
            .RuleFor(x => x.CreatedAt, f => f.Date.Past())
            .RuleFor(x => x.UserId, f => f.PickRandom(createdUsers.Select(x => new Guid(x.Id))));

        var posts = postFaker.Generate(10);

        foreach (var post in posts)
        {
            await _postRepository.Save(post, CancellationToken.None);
        }

        var commentsFaker = new Faker<CommentModel>()
            .RuleFor(x => x.Id, f => f.Random.Guid())
            .RuleFor(x => x.Content, f => f.Random.Words(10))
            .RuleFor(x => x.CreatedAt, f => f.Date.Future())
            .RuleFor(x => x.PostId, f => f.PickRandom(posts.Select(x => x.Id)));

        var comments = commentsFaker.Generate(100);

        foreach (var comment in comments)
        {
            await _commentRepository.Save(comment, CancellationToken.None);
        }

        return Ok();
    }
}