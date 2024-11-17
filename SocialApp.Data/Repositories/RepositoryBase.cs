using MongoDB.Driver;
using SocialApp.Data.Models;

namespace SocialApp.Data.Repositories;

public interface IRepositoryBase<T> where T : ModelBase
{
    IMongoCollection<T> GetCollection();
    Task<T> Save(T entity, CancellationToken cancellationToken);

    Task<T> GetById(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<T>> GetAll(CancellationToken cancellationToken);

    Task Delete(Guid id, CancellationToken cancellationToken);

    Task Update(T entity, CancellationToken cancellationToken);
}

public class RepositoryBase<T> : IRepositoryBase<T> where T : ModelBase
{
    protected readonly IMongoCollection<T> _collection;

    protected RepositoryBase(SocialDbContext dbContext, string collectionName)
    {
        _collection = dbContext.GetCollection<T>(collectionName);
    }

    public IMongoCollection<T> GetCollection() => _collection;

    public async Task<T> Save(T entity, CancellationToken cancellationToken)
    {
        if (entity.Id == Guid.Empty)
        {
            entity.Id = Guid.NewGuid();
            await _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        }
        else
        {
            var filter = Builders<T>.Filter.Eq(e => e.Id, entity.Id);
            await _collection.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = true }, cancellationToken);
        }
        return entity;
    }

    public virtual async Task<T> GetById(Guid id, CancellationToken cancellationToken)
    {
        return await _collection.Find(e => e.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<T>> GetAll(CancellationToken cancellationToken)
    {
        return await _collection.Find(_ => true)
            .Sort(Builders<T>.Sort.Descending(a => a.CreatedAt))
            .ToListAsync(cancellationToken);
    }

    public async Task Update(T entity, CancellationToken cancellationToken)
    {
        var filter = Builders<T>.Filter.Eq(e => e.Id, entity.Id);
        await _collection.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = true }, cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        await _collection.DeleteOneAsync(e => e.Id == id, cancellationToken);
    }
}