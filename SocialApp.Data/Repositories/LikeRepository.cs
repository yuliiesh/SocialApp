using MongoDB.Driver;
using SocialApp.Data.Models;

namespace SocialApp.Data.Repositories;

public interface ILikeRepository : IRepositoryBase<ReactionModel>
{
    Task<int> GetLikesCountForPost(Guid postId, CancellationToken cancellationToken);

    Task UnlikePost(Guid postId, Guid userId, CancellationToken cancellationToken);

    Task<Dictionary<Guid, HashSet<Guid>>> GetUsersPostLikes(CancellationToken cancellationToken);
}

public class LikeRepository : RepositoryBase<ReactionModel>, ILikeRepository
{
    public LikeRepository(SocialDbContext dbContext)
        : base(dbContext, "reactions") { }

    public async Task<int> GetLikesCountForPost(Guid postId, CancellationToken cancellationToken)
    {
        var filter = Builders<ReactionModel>.Filter.Eq(x => x.PostId, postId);
        return (int)(await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken));
    }

    public async Task UnlikePost(Guid postId, Guid userId, CancellationToken cancellationToken)
    {
        var filter = Builders<ReactionModel>.Filter.And(
            Builders<ReactionModel>.Filter.Eq(x => x.PostId, postId),
            Builders<ReactionModel>.Filter.Eq(x => x.UserId, userId));

        await _collection.DeleteOneAsync(filter, cancellationToken);
    }

    public async Task<Dictionary<Guid, HashSet<Guid>>> GetUsersPostLikes(CancellationToken cancellationToken)
    {
        var result = await _collection
            .Aggregate()
            .Match(x => x.PostId != null)
            .Group(x => x.PostId.Value,
                x =>
                    new
                    {
                        PostId = x.Key,
                        Users = x.Select(reaction => reaction.UserId).ToList()
                    })
            .ToListAsync(cancellationToken);

        return result.ToDictionary(x => x.PostId, x => x.Users.ToHashSet());
    }
}