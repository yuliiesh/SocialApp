using Microsoft.AspNetCore.Identity;
using SocialApp.Data;

namespace SocialApp.Api;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddScoped<SocialDbContext>(provider =>
        {
            var connectionString = provider.GetService<IConfiguration>().GetConnectionString("DefaultConnection");
            return new SocialDbContext(connectionString, "SocialApp");
        });
        return services;
    }

    public static IServiceCollection AddUserAuthorization(this IServiceCollection services)
    {
        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<UserDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication();
        services.AddAuthorization();

        return services;
    }
}