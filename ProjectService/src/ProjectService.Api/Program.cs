using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using ProjectService.Api.Common;
using ProjectService.Application;
using ProjectService.Infrastructure;


var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddApiServices(builder.Configuration);
    builder.Services.AddApplicationServices(builder.Configuration);
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddHealthChecks();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
    });
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseHttpsRedirection();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();


    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                var result = new Dictionary<string, object>();
                foreach (var entry in report.Entries)
                {
                    result[entry.Key] = new
                    {
                        status = entry.Value.Status.ToString(),
                        description = entry.Value.Description,
                        exception = entry.Value.Exception?.Message ?? "none",
                        duration = entry.Value.Duration.ToString()
                    };
                }

                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(System.Text.Json.JsonSerializer.
                Serialize(result, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));
            }
        });
    });



    app.Run();
}