using MongoDB.Driver;
using SocialApp.Data.Models;

namespace SocialApp.Data.Repositories;

public interface ILikeRepository : IRepositoryBase<ReactionModel>
{
    Task<int> GetLikesCountForPost(Guid postId, CancellationToken cancellationToken);
    Task AddLikeToPost(Guid postId, Guid userId, CancellationToken cancellationToken);
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

    public Task AddLikeToPost(Guid postId, Guid userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}