using Amazon.CDK;
using Utilities.RecipeRandomizer.Infrastructure.CDK.Constants.ApiGateway;

namespace GoogleCustomSearchService.Build.Infrastructure;

sealed class Program
{
    public static void Main(string[] args)
    {
        var app = new App();

        GoogleCustomSearchServiceElasticBeanstalkStack ebStack = new GoogleCustomSearchServiceElasticBeanstalkStack(app, "google-custom-search-service-elastic-beanstalk-stack", new GoogleCustomSearchServiceElasticBeanstalkStackProps 
        {
            ApplicationName = "GoogleCustomSearchService",
            //Vpc = DatabaseStack.Vpc,
            Env = new Amazon.CDK.Environment
            {
                Account = System.Environment.GetEnvironmentVariable("PROJECTS_AWS_DEFAULT_ACCOUNT", EnvironmentVariableTarget.User),
                Region = "us-east-1"
            }
        });

        GoogleCustomSearchServiceApiGatewayStack apiGatewayStack = new GoogleCustomSearchServiceApiGatewayStack(app, "google-custom-search-service-api-gateway-stack", new GoogleCustomSearchServiceApiGatewayProps
        {
            BaseUrl = "http://projects.eba-kry82eby.us-east-1.elasticbeanstalk.com",
            RestApiId = ApiGatewayExportKeys.RecipeRandomizerApiGatewayRestApiId,
            RootResourceId = ApiGatewayExportKeys.RecipeRandomizerApiGatewayRootResourceId
        });

        app.Synth();
    }
}
