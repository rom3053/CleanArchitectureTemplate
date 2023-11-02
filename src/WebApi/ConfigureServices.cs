using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.WebApi.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddWebAPIServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
