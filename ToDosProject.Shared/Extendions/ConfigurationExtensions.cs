using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using ToDosProject.Domain.Constants;
using ToDosProject.Shared.Handler;
using ToDosProject.Shared.Services;

namespace ToDosProject.Shared.Extendions
{
    public static class ConfigurationExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            // Add handlers to the authentication
            services.AddAuthorizationCore();
            services.AddScoped<CookieHandler>();
            services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();
            services.AddScoped(x => (CookieAuthenticationStateProvider)x.GetRequiredService<AuthenticationStateProvider>());

            services.AddHttpClient<ApiServiceClient>(
                client =>
                {
                    // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
                    // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
                    client.BaseAddress = new($"https://{AppConfiguration.API}");
                })
                .AddHttpMessageHandler<CookieHandler>();

            services.AddMudServices();

        }
    }
}
