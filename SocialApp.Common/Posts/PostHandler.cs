using System.Collections.Immutable;
using SocialApp.Common.Posts.Create;
using SocialApp.Data.Models;
using SocialApp.Data.Repositories;

namespace SocialApp.Common.Posts;

public interface IPostHandler
{
    Task<IList<PostDto>> GetPosts(CancellationToken cancellationToken);
    Task<IList<PostDto>> GetPosts(Guid userId, CancellationToken cancellationToken);
    Task<CreatePostResponse> CreatePost(CreatePostRequest request, CancellationToken cancellationToken);
    Task DeletePost(Guid id, CancellationToken cancellationToken);
}

public class PostHandler : IPostHandler
{
    private readonly IPostRepository _postRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly ILikeRepository _likeRepository;

    public PostHandler(
        IPostRepository postRepository,
        IProfileRepository profileRepository,
        ILikeRepository likeRepository)
    {
        _postRepository = postRepository;
        _profileRepository = profileRepository;
        _likeRepository = likeRepository;
    }


    public async Task<IList<PostDto>> GetPosts(CancellationToken cancellationToken)
    {
        var posts = await _postRepository.GetAll(cancellationToken);

        if (!posts.Any())
        {
            return [];
        }

        return await FillPostsData(posts, cancellationToken);
    }

    public async Task<IList<PostDto>> GetPosts(Guid userId, CancellationToken cancellationToken)
    {
        var posts = await _postRepository.GetAll(userId, cancellationToken);

        if (!posts.Any())
        {
            return [];
        }

        return await FillPostsData(posts, cancellationToken);
    }

    public async Task<CreatePostResponse> CreatePost(CreatePostRequest request, CancellationToken cancellationToken)
    {
        var createdPost = await _postRepository.Save(request.ToPostModel(), cancellationToken);

        var profile = await _profileRepository.GetById(request.UserId, cancellationToken);

        return createdPost.ToCreatePostResponse(profile);
    }

    public async Task DeletePost(Guid id, CancellationToken cancellationToken)
    {
        await _postRepository.Delete(id, cancellationToken);
    }

    private async Task<IList<PostDto>> FillPostsData(IReadOnlyCollection<PostModel> posts, CancellationToken cancellationToken)
    {
        var userIds = posts.Select(p => p.UserId).Where(x => x != Guid.Empty).ToImmutableHashSet();

        var profiles = await _profileRepository.GetProfiles(userIds, cancellationToken);

        var likes = await _likeRepository.GetUsersPostLikes(cancellationToken);

        return posts.Select(x => x.ToDto(profiles[x.UserId], likes.GetValueOrDefault(x.Id, []))).ToList();
    }
}