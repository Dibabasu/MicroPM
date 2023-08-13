using ProjectService.Api.Filters;
using ProjectService.Application;
using ProjectService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers(
        options =>
        options.Filters.Add(new GlobalExceptionFilter())
    );
    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructure(builder.Configuration);

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
    app.MapControllers();

    app.Run();
}