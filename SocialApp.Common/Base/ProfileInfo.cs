namespace SocialApp.Common.Base;

public class ProfileInfo
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public required Guid UserId { get; set; }
    public string ProfilePicture { get; set; }
}