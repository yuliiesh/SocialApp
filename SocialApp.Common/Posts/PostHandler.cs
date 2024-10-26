using System.Collections.Immutable;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using SocialApp.Common.Posts.Create;
using SocialApp.Data.Repositories;

namespace SocialApp.Common.Posts;

public interface IPostHandler
{
    Task<IList<PostDto>> GetPosts(CancellationToken cancellationToken);

    Task<CreatePostResponse> CreatePost(CreatePostRequest request, CancellationToken cancellationToken);
}

public class PostHandler : IPostHandler
{
    private readonly IPostRepository _postRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly ILogger<PostHandler> _logger;

    public PostHandler(
        IPostRepository postRepository,
        IProfileRepository profileRepository,
        ILogger<PostHandler> logger)
    {
        _postRepository = postRepository;
        _profileRepository = profileRepository;
        _logger = logger;
    }


    public async Task<IList<PostDto>> GetPosts(CancellationToken cancellationToken)
    {
        var posts = await _postRepository.GetAll(cancellationToken);

        if (!posts.Any())
        {
            return [];
        }

        var userIds = posts.Select(p => p.UserId).Where(x => x != Guid.Empty).ToImmutableHashSet();

        var profiles = await _profileRepository.GetProfiles(userIds, cancellationToken);

        return posts.Select(x => x.ToDto(profiles[x.UserId])).ToList();
    }

    public async Task<CreatePostResponse> CreatePost(CreatePostRequest request, CancellationToken cancellationToken)
    {
        var createdPost = await _postRepository.Save(request.ToPostModel(), cancellationToken);

        var profile = await _profileRepository.GetById(request.UserId, cancellationToken);

        return createdPost.ToCreatePostResponse(profile);
    }
}