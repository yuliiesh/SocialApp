namespace SocialApp.Common.Profiles.Create;

public class CreateProfileRequest
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
}