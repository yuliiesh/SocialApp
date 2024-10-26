using MongoDB.Driver;
using SocialApp.Data.Models;

namespace SocialApp.Data.Repositories;

public interface ICommentRepository : IRepositoryBase<CommentModel>
{
    Task<IReadOnlyCollection<CommentModel>> GetCommentsByPostId(Guid postId, CancellationToken cancellationToken);
    Task<int> GetCommentsCount(Guid postId, CancellationToken cancellationToken);
}

public class CommentRepository : RepositoryBase<CommentModel>, ICommentRepository
{
    public CommentRepository(SocialDbContext dbContext)
        : base(dbContext, "comments")
    {
    }

    public async Task<IReadOnlyCollection<CommentModel>> GetCommentsByPostId(Guid postId, CancellationToken cancellationToken)
    {
        var filter = Builders<CommentModel>.Filter.Eq(c => c.PostId, postId);
        var collection = await _collection.FindAsync<CommentModel>(filter, cancellationToken: cancellationToken);
        return await collection.ToListAsync(cancellationToken);
    }

    public async Task<int> GetCommentsCount(Guid postId, CancellationToken cancellationToken)
    {
        var filter = Builders<CommentModel>.Filter.Eq(c => c.PostId, postId);
        var count = await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        return (int)count;
    }
}