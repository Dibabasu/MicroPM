using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ProjectService.Api.Common.Interfaces;
using ProjectService.Api.Common.Services;
using ProjectService.Api.Filters;

namespace ProjectService.Api.Common;

public static class ConfigureServices
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers(
            options => options.Filters.Add(new GlobalExceptionFilter()));

        services.AddHttpContextAccessor();
        services.AddCognitoIdentity();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.Authority = configuration.GetValue<string>("cognito:authority")!;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateAudience = false
            };
        });
        services.AddScoped<ICustomClaimService, CustomClaimService>();
        return services;
    }
}