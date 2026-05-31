using Microsoft.Extensions.DependencyInjection;
using Obscura.FinanceTracker.Client.Categories;

namespace Obscura.FinanceTracker.Client;

public static class DependencyInjection
{
    public static IServiceCollection AddClientServices(this IServiceCollection services)
    {
        services.AddScoped<CategoryClient>();

        return services;
    }
}