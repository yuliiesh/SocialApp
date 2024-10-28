using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SocialApp.Client.Components.Base;
using SocialApp.Client.Extensions;
using SocialApp.Client.Services;
using SocialApp.Common.Posts;
using SocialApp.Common.Profiles;

namespace SocialApp.Client.Pages.Profile;

public partial class ProfilePage : LoadingComponent
{
    private IList<PostDto> _posts;
    private ProfileDto _profile;
    private IBrowserFile _profileImage;
    private string _profileImagePreview;

    [Inject] private IPostService PostService { get; set; }
    [Inject] private ILocalStorageService LocalStorage { get; set; }
    [Inject] private IImageService ImageService { get; set; }
    [Inject] private IProfileService ProfileService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await HandleLoadAction(async () =>
        {
            _profile = await LocalStorage.GetProfile();
            _posts = await PostService.GetAllPosts(_profile.UserId, CancellationToken.None);
        });
    }

    private void PostCreated(PostDto post)
    {
        _posts.Insert(0, post);
        StateHasChanged();
    }

    private async Task PostDeleted(PostDto post)
    {
        await PostService.DeletePost(post.Id);
        _posts.Remove(post);
        StateHasChanged();
    }

    private async Task OnProfilePhotoChange(InputFileChangeEventArgs e)
    {
        _profileImage = e.File;
        await using var stream = _profileImage.OpenReadStream();
        var buffer = new byte[_profileImage.Size];
        await stream.ReadAsync(buffer.AsMemory(0, (int) _profileImage.Size));

        _profileImagePreview = $"data:{_profileImage.ContentType};base64,{Convert.ToBase64String(buffer)}";

        using var content = new MultipartFormDataContent();
        var profilePhotoContent = new StreamContent(_profileImage.OpenReadStream());
        content.Add(profilePhotoContent, "mainPhoto", _profileImage.Name);

        var storedPhoto = await ImageService.Upload(_profile.UserId, content, default);

        _profile.ProfilePicture = storedPhoto.First().StoredFileName;

        await LocalStorage.SetItem("profile", _profile);
        await ProfileService.Update(_profile);
        StateHasChanged();
    }
}