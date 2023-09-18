using System.Net.Mime;
using System.Xml;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using ProjectService.Api.Filters;
using ProjectService.Application;
using ProjectService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers(
        options =>
        options.Filters.Add(new GlobalExceptionFilter())
    );
    builder.Services.AddApplicationServices(builder.Configuration);
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddHealthChecks();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
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