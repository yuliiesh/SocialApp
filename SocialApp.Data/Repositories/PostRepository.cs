using MongoDB.Driver;
using SocialApp.Data.Models;

namespace SocialApp.Data.Repositories;

public interface IPostRepository : IRepositoryBase<PostModel>
{
    Task<IReadOnlyCollection<PostModel>> GetAll(Guid userId, CancellationToken cancellationToken);
}

public class PostRepository : RepositoryBase<PostModel>, IPostRepository
{
    public PostRepository(SocialDbContext dbContext)
        : base(dbContext, "posts") { }

    public async Task<IReadOnlyCollection<PostModel>> GetAll(Guid userId, CancellationToken cancellationToken)
    {
        var filter = Builders<PostModel>.Filter.Eq(p => p.UserId, userId);
        return await _collection.Find(filter)
            .Sort(Builders<PostModel>.Sort.Descending(a => a.CreatedAt))
            .ToListAsync(cancellationToken);
    }
}