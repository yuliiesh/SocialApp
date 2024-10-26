﻿using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SocialApp.Client.Services;
using SocialApp.Common.Posts;
using SocialApp.Common.Posts.Create;
using SocialApp.Common.Profiles;

namespace SocialApp.Client.Components.Post;

public partial class CreatePost : ComponentBase
{
    private CreatePostRequest _createPostRequest = new();

    [Inject] private IPostService PostService { get; set; }
    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    [Parameter] public Action<PostDto> OnPostCreated { get; set; }
    [Parameter] public ProfileDto Profile { get; set; }

    private async Task Create()
    {
        if (string.IsNullOrEmpty(_createPostRequest.Content))
        {
            return;
        }

        var user = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var id = new Guid(user.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);

        _createPostRequest.UserId = id;

        var createdPost = await PostService.CreatePost(_createPostRequest, CancellationToken.None);
        OnPostCreated(createdPost);
    }
}