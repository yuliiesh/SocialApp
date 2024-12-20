﻿namespace SocialApp.Common.Profiles;

public class ProfileDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public int FriendsCount { get; set; }
    public Guid UserId { get; set; }
    public required string ProfilePicture { get; set; }
    public required ICollection<Guid> Friends { get; set; }
}