using Microsoft.AspNetCore.Components;

namespace SocialApp.Client.Components.Base;

public abstract class LoadingComponent : ComponentBase
{
    protected bool Loading;

    protected async Task HandleLoadAction(Func<Task> action)
    {
        Loading = true;
        await action();
        Loading = false;
    }
}