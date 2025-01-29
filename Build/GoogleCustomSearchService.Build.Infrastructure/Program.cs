using Amazon.CDK;
using Utilities.RecipeRandomizer.Infrastructure.CDK.Constants.ApiGateway;
using Utilities.RecipeRandomizer.Infrastructure.CDK.Helpers;

namespace GoogleCustomSearchService.Build.Infrastructure;

sealed class Program
{
    const string Region = "eu-east-1";

    public static void Main(string[] args)
    {
        var app = new App();

        GoogleCustomSearchServiceElasticBeanstalkStack ebStack = new GoogleCustomSearchServiceElasticBeanstalkStack(app, "google-custom-search-service-elastic-beanstalk-stack", new GoogleCustomSearchServiceElasticBeanstalkStackProps 
        {
            ApplicationName = "GoogleCustomSearchService",
            Env = new Amazon.CDK.Environment
            {
                Account = System.Environment.GetEnvironmentVariable("PROJECTS_AWS_DEFAULT_ACCOUNT", EnvironmentVariableTarget.User),
                Region = Region
            }
        });

        GoogleCustomSearchServiceApiGatewayStack apiGatewayStack = new GoogleCustomSearchServiceApiGatewayStack(app, "google-custom-search-service-api-gateway-stack", new GoogleCustomSearchServiceApiGatewayProps
        {
            BaseUrl = CdkHelpers.GetElasticBeanstalkDomain(ebStack.GooglecustomSearchServiceEbEnvironment.CnamePrefix ?? string.Empty, Region),
            RestApiIdExportKey = ApiGatewayExportKeys.RecipeRandomizerApiGatewayRestApiId,
            RootResourceIdExportKey = ApiGatewayExportKeys.RecipeRandomizerApiGatewayRootResourceId
        });

        apiGatewayStack.AddDependency(ebStack);

        app.Synth();
    }
}
