using MongoDB.Driver;
using SocialApp.Common.Profiles;
using SocialApp.Data.Models;
using SocialApp.Data.Repositories;

namespace SocialApp.Common.Search;

public interface ISearchHandler
{
    Task<IReadOnlyCollection<ProfileDto>> Search(string query, CancellationToken cancellationToken);
}

public class SearchHandler : ISearchHandler
{
    private readonly IProfileRepository _profileRepository;

    public SearchHandler(IProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

    public async Task<IReadOnlyCollection<ProfileDto>> Search(string query, CancellationToken cancellationToken)
    {
        var filter = Builders<ProfileModel>.Filter.Or(
            Builders<ProfileModel>.Filter.Regex(x => x.Username, new MongoDB.Bson.BsonRegularExpression(query, "i")),
            Builders<ProfileModel>.Filter.Regex(x => x.Email, new MongoDB.Bson.BsonRegularExpression(query, "i")),
            Builders<ProfileModel>.Filter.Regex(x => x.FirstName, new MongoDB.Bson.BsonRegularExpression(query, "i")),
            Builders<ProfileModel>.Filter.Regex(x => x.LastName, new MongoDB.Bson.BsonRegularExpression(query, "i"))
        );

        var results = await _profileRepository.GetCollection()
            .Find(filter)
            .ToListAsync(cancellationToken);

        return results.Select(x => x.ToDto()).ToList();
    }
}