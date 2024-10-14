using Microsoft.AspNetCore.Components;
using SocialApp.Common.Authorization;

namespace SocialApp.Client.Components.Authorization;

public partial class RegisterComponent : ComponentBase
{
    private RegisterRequest _registerRequest = new();

    [Parameter] public Action ChangeToLogin { get; set; }
    [Parameter] public Func<RegisterRequest, Task> OnRegister { get; set; }
}