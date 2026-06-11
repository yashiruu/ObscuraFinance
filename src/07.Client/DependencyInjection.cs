using Microsoft.Extensions.DependencyInjection;
using Obscura.FinanceTracker.Client.Features;

namespace Obscura.FinanceTracker.Client;

public static class DependencyInjection
{
    public static IServiceCollection AddClientServices(this IServiceCollection services, string apiBaseUrl)
    {
        services.AddHttpClient<CategoryClient>(client =>
        {
            client.BaseAddress = new Uri(apiBaseUrl);
        });

        services.AddHttpClient<AccountClient>(client =>
        {
            client.BaseAddress = new Uri(apiBaseUrl);
        });
        services.AddHttpClient<TransactionClient>(client =>
        {
            client.BaseAddress = new Uri(apiBaseUrl);
        });

        return services;
    }
}