using System.Collections.Immutable;
using Org.BouncyCastle.X509;
using SocialApp.Data.Models;
using SocialApp.Data.Repositories;

namespace SocialApp.Common.Comments;

public interface ICommentHandler
{
    Task<IReadOnlyCollection<CommentDto>> GetCommentsByPostId(Guid postId, CancellationToken cancellationToken);
    Task<int> GetCommentsCount(Guid postId, CancellationToken cancellationToken);
    Task CreateComment(Guid postId, CommentDto comment, CancellationToken cancellationToken);
}

public class CommentHandler : ICommentHandler
{
    private readonly ICommentRepository _commentRepository;
    private readonly IProfileRepository _profileRepository;

    public CommentHandler(
        ICommentRepository commentRepository,
        IProfileRepository profileRepository)
    {
        _commentRepository = commentRepository;
        _profileRepository = profileRepository;
    }

    public async Task<IReadOnlyCollection<CommentDto>> GetCommentsByPostId(Guid postId, CancellationToken cancellationToken)
    {
        var comments = await _commentRepository.GetCommentsByPostId(postId, cancellationToken);

        var userIds = comments.Select(x => x.UserId).ToImmutableHashSet();

        var profiles = await _profileRepository.GetProfiles(userIds, cancellationToken);

        return comments.Select(x => x.ToDto(profiles[x.UserId])).ToList();
    }

    public async Task<int> GetCommentsCount(Guid postId, CancellationToken cancellationToken) =>
        await _commentRepository.GetCommentsCount(postId, cancellationToken);

    public async Task CreateComment(Guid postId, CommentDto comment, CancellationToken cancellationToken)
    {
        var model = new CommentModel
        {
            PostId = postId,
            CreatedAt = DateTime.Now,
            Content = comment.Content,
            UserId = comment.ProfileInfo.UserId,
            Id = Guid.NewGuid(),
        };

        await _commentRepository.Save(model, cancellationToken);
    }
}