using SocialApp.Data.Repositories;

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

    public FriendHandler(IProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

    public async Task AddFriend(Guid userId, Guid friendId, CancellationToken cancellationToken)
    {
        var profile = await _profileRepository.GetById(userId, cancellationToken);

        profile.Friends ??= [];

        if (!profile.Friends.Contains(friendId))
        {
            profile.Friends.Add(friendId);
            await _profileRepository.Update(profile, cancellationToken);
        }
    }

    public async Task<IReadOnlyCollection<FriendDto>> GetFriendsInfo(Guid userId, CancellationToken cancellationToken)
    {
        var profile = await _profileRepository.GetById(userId, cancellationToken);

        var friends = await _profileRepository.GetProfiles([.. profile.Friends ?? []], cancellationToken);

        return friends.Values.Select(x => new FriendDto
        {
            Profile = new()
            {
                Username = x.Username,
                FirstName = x.FirstName,
                UserId = x.UserId,
                LastName = x.LastName,
            }
        }).ToList();
    }

    public async Task RemoveFriend(Guid userId, Guid friendId, CancellationToken cancellationToken)
    {
        var profile = await _profileRepository.GetById(userId, cancellationToken);

        var removed = profile.Friends.Remove(friendId);

        if (removed)
        {
            await _profileRepository.Update(profile, cancellationToken);
        }
    }
}