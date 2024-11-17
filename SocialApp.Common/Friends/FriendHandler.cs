using System.Collections.Immutable;
using Microsoft.Extensions.Logging;
using Neo4jClient;
using SocialApp.Common.Base;
using SocialApp.Data.Repositories;
using SocialApp.GraphData;

namespace SocialApp.Common.Friends;

public interface IFriendHandler
{
    Task AddFriend(Guid userId, Guid friendId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<FriendDto>> GetFriendsInfo(Guid userId, CancellationToken cancellationToken);
    Task RemoveFriend(Guid userId, Guid friendId, CancellationToken cancellationToken);
}

public class FriendHandler : IFriendHandler
{
    private readonly IProfileRepository _profileRepository;
    private readonly IGraphClient _graphClient;
    private readonly ILogger<FriendHandler> _log;

    public FriendHandler(IProfileRepository profileRepository, Neo4jDbContext _graphContext, ILogger<FriendHandler> log)
    {
        _profileRepository = profileRepository;
        _log = log;
        _graphClient = _graphContext.Client;
    }

    public async Task AddFriend(Guid userId, Guid friendId, CancellationToken cancellationToken)
    {
        try
        {
            var profile = await _profileRepository.GetById(userId, cancellationToken);

            await _graphClient.Cypher
                .Match("(user:User {UserId: $userId})", "(friend:User {UserId: $friendId})")
                .Create("(user)-[:FRIENDS_WITH]->(friend)")
                .WithParams(new { userId, friendId })
                .ExecuteWithoutResultsAsync();

            profile.Friends ??= [];

            if (!profile.Friends.Contains(friendId))
            {
                profile.Friends.Add(friendId);
                await _profileRepository.Update(profile, cancellationToken);
            }
        }
        catch (Exception e)
        {
            _log.LogError(e, e.Message);
            return;
        }
    }

    public async Task<IReadOnlyCollection<FriendDto>> GetFriendsInfo(Guid userId, CancellationToken cancellationToken)
{
    var profile = await _profileRepository.GetById(userId, cancellationToken);

    var friends = await _profileRepository.GetProfiles(new HashSet<Guid>(profile.Friends ?? []), cancellationToken);

    var friendDtos = new List<FriendDto>();

    foreach (var friend in friends.Values)
    {
        var mutualFriendIds = await _graphClient.Cypher
            .Match("(user:User)-[:FRIENDS_WITH]->(friend:User)-[:FRIENDS_WITH]->(mutualFriend:User)")
            .Where((FriendRelationship rel) => rel.UserId == userId && friend.Friends.Contains(rel.FriendId))
            .Return(mutualFriend => mutualFriend.As<Guid>())
            .ResultsAsync;

        var mutualFriends = await _profileRepository.GetProfiles(mutualFriendIds.ToImmutableHashSet(), cancellationToken);

        friendDtos.Add(new FriendDto
        {
            Profile = new()
            {
                Username = friend.Username,
                FirstName = friend.FirstName,
                UserId = friend.UserId,
                LastName = friend.LastName,
                ProfilePicture = friend.ProfilePicture
            },
            MutualFriends = mutualFriends.Values.Select(x => new ProfileInfo
            {
                Username = x.Username,
                FirstName = x.FirstName,
                UserId = x.UserId,
                LastName = x.LastName,
                ProfilePicture = x.ProfilePicture
            }).ToList()
        });
    }

    return friendDtos;
}

    public async Task RemoveFriend(Guid userId, Guid friendId, CancellationToken cancellationToken)
    {
        var profile = await _profileRepository.GetById(userId, cancellationToken);

        var removed = profile.Friends.Remove(friendId);

        if (removed)
        {
            await _graphClient.Cypher
                .Match("(user:User {UserId: $userId})-[rel:FRIENDS_WITH]->(friend:User {UserId: $friendId})")
                .Delete("rel")
                .WithParams(new { userId, friendId })
                .ExecuteWithoutResultsAsync();
            await _profileRepository.Update(profile, cancellationToken);
        }
    }
}