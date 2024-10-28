using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SocialApp.Common.Profiles;

namespace SocialApp.Client.Components.Profile;

public partial class ProfileDetailsComponent : ComponentBase
{
    [Parameter] public ProfileDto Profile { get; set; }
    [Parameter] public string ProfileImagePreview { get; set; }
    [Parameter] public EventCallback<InputFileChangeEventArgs> OnProfilePhotoChange { get; set; }
}