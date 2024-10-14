using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SocialApp.Client;
using SocialApp.Client.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(provider =>
{
    var configuration = provider.GetService<IConfiguration>();

    var uriString = configuration["ApiSettings:BaseUrl"];
    var httpClient = new HttpClient { BaseAddress = new(uriString!), };
    return httpClient;
});

builder.Services.AddServices();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, UserAuthenticationStateProvider>();
builder.Services.AddScoped<UserAuthenticationStateProvider>();

await builder.Build().RunAsync();