using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SocialApp.Data;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjectionExtensions).Assembly;

        var types = assembly.GetTypes();

        var repositoryInterfaces = types
            .Where(t => t.IsInterface && t.Name.StartsWith('I') && t.Name.EndsWith("Repository"));

        foreach (var repositoryInterface in repositoryInterfaces)
        {
            // Знайти реалізацію для кожного інтерфейсу
            var implementation = types.FirstOrDefault(t =>
                t.IsClass && !t.IsAbstract &&
                repositoryInterface.IsAssignableFrom(t));

            if (implementation != null)
            {
                // Зареєструвати інтерфейс з реалізацією
                services.AddScoped(repositoryInterface, implementation);
            }
        }

        return services;
    }

    public static IServiceCollection AddUserDatabase(this IServiceCollection services, IConfiguration configuration) =>
        services.AddUserDatabase(configuration.GetConnectionString("UserStorage"));

    public static IServiceCollection AddUserDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<UserDbContext>(options =>
            options.UseMySQL(connectionString));

        return services;
    }
}