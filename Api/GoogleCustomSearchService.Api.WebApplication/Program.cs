using Serilog;
using GoogleCustomSearchService.Api.Domain.Clients.Interfaces;
using GoogleCustomSearchService.Api.Domain.Clients;
using GoogleCustomSearchService.Api.Domain.Queries;
using GoogleCustomSearchService.Api.WebApplication.ExceptionHandler;
using Amazon.CloudWatchLogs;
using Amazon.Runtime;
using Serilog.Sinks.AwsCloudWatch;
using Amazon;
using Utilities.ConfigurationManager.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddMvcCore().AddApiExplorer();
builder.Services.AddOpenApiDocument(config => 
{
    config.Title = "Google Custom Search Engine Microservice";
});

builder.Services.AddProblemDetails().AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetGoogleResultsQuery).Assembly));
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddTransient<IGoogleCustomSearchClient, GoogleCustomSearchClient>();

var app = builder.Build();

if(builder.Configuration.GetBoolValue("AwsCloudwatchLogging:Enabled") == true)
{
    var client = new AmazonCloudWatchLogsClient(new BasicAWSCredentials(builder.Configuration.GetStringValue("AwsCloudwatchLogging:AccessKey"), builder.Configuration.GetStringValue("AwsCloudwatchLogging:SecretKey")), RegionEndpoint.USEast1);

    Log.Logger = new LoggerConfiguration().WriteTo.AmazonCloudWatch(
        logGroup: builder.Configuration.GetStringValue("AwsCloudwatchLogging:LogGroup"),
        logStreamPrefix: builder.Configuration.GetStringValue("AwsCloudwatchLogging:LogStreamPrefix"),
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Verbose,
        createLogGroup: true,
        appendUniqueInstanceGuid: true,
        appendHostName: false,
        logGroupRetentionPolicy: LogGroupRetentionPolicy.ThreeDays,
        cloudWatchClient: client).CreateLogger();
}
else
{
    Log.Logger = new LoggerConfiguration().WriteTo.File("./Logs/logs-", rollingInterval: RollingInterval.Day).MinimumLevel.Debug().CreateLogger();
}

if(app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.UseStatusCodePages();
app.UseExceptionHandler();

app.MapControllers();

app.Run();
