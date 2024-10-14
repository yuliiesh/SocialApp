﻿using Microsoft.AspNetCore.Components;
using SocialApp.Client.Components.Base;
using SocialApp.Client.Services;
using SocialApp.Common.Posts;

namespace SocialApp.Client.Pages;

public partial class ProfilePage : LoadingComponent
{
    private bool _loading;

    private IList<PostDto> _posts;

    [Inject]
    private IPostService PostService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await HandleLoadAction(async () =>
        {
            _posts = await PostService.GetAllPosts(CancellationToken.None);
        });

        await base.OnInitializedAsync();
    }
}