using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SocialApp.Client.Services;

namespace SocialApp.Client.Components.Profile;

public partial class ProfileImageComponent : ComponentBase
{
    [Parameter] public string ProfileImagePreview { get; set; }
    [Parameter] public string ProfilePicture { get; set; }
    [Parameter] public Guid UserId { get; set; }
    [Parameter] public EventCallback<InputFileChangeEventArgs> OnProfilePhotoChange { get; set; }
    [Inject] private IImageService ImageService { get; set; }
}