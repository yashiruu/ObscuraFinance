using Microsoft.Extensions.DependencyInjection;
using Obscura.FinanceTracker.Client.Features;

namespace Obscura.FinanceTracker.Client;

/// <summary>
/// Registers the typed HTTP clients used to consume the Web API.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers <see cref="Features.CategoryClient"/>, <see cref="Features.AccountClient"/>,
    /// <see cref="Features.TransactionClient"/>, and <see cref="Features.DashboardClient"/> as typed HTTP clients.
    /// </summary>
    /// <param name="services">The service collection to add the clients to.</param>
    /// <param name="apiBaseUrl">The base address of the Web API.</param>
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

        services.AddHttpClient<DashboardClient>(client =>
        {
            client.BaseAddress = new Uri(apiBaseUrl);
        });

        return services;
    }
}