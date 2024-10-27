using SocialApp.Data.Models;
using SocialApp.Data.Repositories;

namespace SocialApp.Common.Likes;

public interface ILikeHandler
{
    Task LikePost(Guid postId, Guid userId, CancellationToken cancellationToken);

    Task UnlikePost(Guid postId, Guid userId, CancellationToken cancellationToken);
}

public class LikeHandler : ILikeHandler
{
    private readonly ILikeRepository _likeRepository;

    public LikeHandler(ILikeRepository likeRepository)
    {
        _likeRepository = likeRepository;
    }

    public async Task LikePost(Guid postId, Guid userId, CancellationToken cancellationToken)
    {
        var model = new ReactionModel
        {
            Id = Guid.NewGuid(),
            PostId = postId,
            UserId = userId,
            CreatedAt = DateTime.Now,
        };

        await _likeRepository.Save(model, cancellationToken);
    }

    public async Task UnlikePost(Guid postId, Guid userId, CancellationToken cancellationToken)
    {
        await _likeRepository.UnlikePost(postId, userId, cancellationToken);
    }
}