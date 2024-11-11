using Serilog;
using GoogleCustomSearchService.Api.Domain.Clients.Interfaces;
using GoogleCustomSearchService.Api.Domain.Clients;
using GoogleCustomSearchService.Api.Domain.Queries;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddMvcCore().AddApiExplorer();
builder.Services.AddOpenApiDocument(config => 
{
    config.Title = "Google Custom Search Engine Microservice";
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetGoogleResultsQuery).Assembly));
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddTransient<IGoogleCustomSearchClient, GoogleCustomSearchClient>();

var app = builder.Build();

Log.Logger = new LoggerConfiguration().WriteTo.File("./Logs/logs-", rollingInterval: RollingInterval.Day).MinimumLevel.Debug().CreateLogger();

if(app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.MapControllers();

app.Run();
