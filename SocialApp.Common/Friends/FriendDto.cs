using SocialApp.Common.Base;

namespace SocialApp.Common.Friends;

public class FriendDto
{
    public ProfileInfo Profile { get; set; }

    public List<ProfileInfo> MutualFriends { get; set; }
}