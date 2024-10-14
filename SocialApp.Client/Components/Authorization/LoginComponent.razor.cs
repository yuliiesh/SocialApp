using Microsoft.AspNetCore.Components;
using SocialApp.Common.Authorization;

namespace SocialApp.Client.Components.Authorization;

public partial class LoginComponent : ComponentBase
{
    private LoginRequest _loginRequest = new();

    [Parameter] public Action ChangeToRegistration { get; set; }
    [Parameter] public Func<LoginRequest, Task> OnLogin { get; set; }
}