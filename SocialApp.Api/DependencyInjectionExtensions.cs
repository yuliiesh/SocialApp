using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialApp.Data;

namespace SocialApp.Api;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddScoped<SocialDbContext>(provider =>
        {
            var connectionString = provider.GetRequiredService<IConfiguration>().GetConnectionString("SocialApp");
            return new SocialDbContext(connectionString, "SocialApp");
        });
        return services;
    }

    public static IServiceCollection AddUserAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<UserDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication();
        services.AddAuthorization();

        return services;
    }
}