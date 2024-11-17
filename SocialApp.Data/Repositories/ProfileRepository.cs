using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using SocialApp.Data.Models;

namespace SocialApp.Data.Repositories;

public interface IProfileRepository : IRepositoryBase<ProfileModel>
{
    Task<ProfileModel> Get(string email, CancellationToken cancellationToken);
    Task<ProfileModel> GetByUsername(string username, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<ProfileModel>> Get(IReadOnlyCollection<Guid> profileIds, CancellationToken cancellationToken);
    Task<ImmutableDictionary<Guid, ProfileModel>> GetProfiles(ISet<Guid> userIds, CancellationToken cancellationToken);
}

public sealed class ProfileRepository : RepositoryBase<ProfileModel>, IProfileRepository
{
    public ProfileRepository(SocialDbContext dbContext)
        : base(dbContext, "profiles") { }

    public override async Task<ProfileModel> GetById(Guid id, CancellationToken cancellationToken)
    {
        var filter = Builders<ProfileModel>.Filter.Eq(x => x.UserId, id);
        return await _collection.Find(filter).SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<ProfileModel> Get(string email, CancellationToken cancellationToken)
    {
        var filter = Builders<ProfileModel>.Filter.Eq(p => p.Email, email);
        return await _collection.Find(filter).SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<ProfileModel> GetByUsername(string username, CancellationToken cancellationToken)
    {
        var filter = Builders<ProfileModel>.Filter.Eq(p => p.Username, username);
        return await _collection.Find(filter).SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<ProfileModel>> Get(IReadOnlyCollection<Guid> profileIds, CancellationToken cancellationToken)
    {
        var filter = Builders<ProfileModel>.Filter.In(x => x.UserId, profileIds);
        return await _collection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<ImmutableDictionary<Guid, ProfileModel>> GetProfiles(ISet<Guid> userIds, CancellationToken cancellationToken)
    {
        var filter = Builders<ProfileModel>.Filter.In(p => p.UserId, userIds);
        var cursor = await _collection
            .FindAsync(filter, cancellationToken: cancellationToken);

        var result = await cursor.ToListAsync(cancellationToken);

        return result.ToImmutableDictionary(x => x.UserId, x => x);
    }
}