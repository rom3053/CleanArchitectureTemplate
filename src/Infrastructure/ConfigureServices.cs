using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Infrastructure.Persistence.Interceptors;
using CleanArchitectureTemplate.Infrastructure.Persistence;
using CleanArchitectureTemplate.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CleanArchitectureTemplate.Infrastructure.Configurations;
using CleanArchitectureTemplate.Application.Common.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var config = GetCleanArchitectureTemplateConfig(configuration);

        services.Configure<IISServerOptions>(options => options.MaxRequestBodySize = int.MaxValue);
        services.Configure<KestrelServerOptions>(options => options.Limits.MaxRequestBodySize = int.MaxValue);
        services.Configure<FormOptions>(options => options.MultipartBodyLengthLimit = int.MaxValue);

        services.AddSingleton(typeof(CleanArchitectureTemplateConfig), config);

        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        if (config.UseInMemoryDatabase)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("CleanArchitectureTemplateDb"));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(config.ConnectionStrings.DefaultConnection,
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ApplicationDbContextInitialiser>();

        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddTransient<IDateTime, DateTimeService>();
        //services.AddTransient<IIdentityService, IdentityService>();

        services.AddAuthorization(AddAuthorizationOptions);
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = config.Jwt.Issuer,
                ValidAudience = config.Jwt.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Jwt.TokenKey)),
                ClockSkew = TimeSpan.FromSeconds(5)
            };
        });

        return services;
    }

    private static void AddAuthorizationOptions(AuthorizationOptions options)
    {
        options.AddPolicy(Policies.AdminRights, policy => policy.RequireRole(Roles.Admin));
    }

    private static CleanArchitectureTemplateConfig GetCleanArchitectureTemplateConfig(IConfiguration configuration)
    {
        var config = new CleanArchitectureTemplateConfig();
        configuration.GetSection(CleanArchitectureTemplateConfig.ConfigName).Bind(config);

        return config;
    }
}
