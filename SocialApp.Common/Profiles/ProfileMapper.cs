﻿using SocialApp.Data.Models;

namespace SocialApp.Common.Profiles;

public static class ProfileMapper
{
    public static ProfileDto ToDto(this ProfileModel model) =>
        new()
        {
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Username = model.Username,
            UserId = model.UserId,
            FriendsCount = model.Friends.Count,
            Friends = model.Friends,
            ProfilePicture = model.ProfilePicture,
        };
}