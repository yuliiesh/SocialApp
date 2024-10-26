using Microsoft.Extensions.DependencyInjection;

namespace SocialApp.Common;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjectionExtensions).Assembly;

        var types = assembly.GetTypes();

        var repositoryInterfaces = types
            .Where(t => t.IsInterface && t.Name.StartsWith('I') && t.Name.EndsWith("Handler"));

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
}