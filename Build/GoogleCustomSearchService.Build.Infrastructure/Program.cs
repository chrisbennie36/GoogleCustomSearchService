using System.Data.Common;
using Amazon.CDK;

namespace GoogleCustomSearchService.Build.Infrastructure;

sealed class Program
{
    public static void Main(string[] args)
    {
        var app = new App();

        //DatabaseMigrationLambdaStack dbMigrationLambdaStack = new DatabaseMigrationLambdaStack(app, "database-migration-lambda-stack");

        DatabaseStack dbStack = new DatabaseStack(app, "database-stack", new DatabaseStackProps
        {
            //MigrationLambda = dbMigrationLambdaStack.DatabaseMigrationLambda,
            Env = new Amazon.CDK.Environment
            {
                Account = System.Environment.GetEnvironmentVariable("PROJECTS_AWS_DEFAULT_ACCOUNT", EnvironmentVariableTarget.User),
                Region = "us-east-1",
                //Region = System.Environment.GetEnvironmentVariable("PROJECTS_AWS_DEFAULT_REGION", EnvironmentVariableTarget.User)
            }
        });

        GoogleCustomSearchServiceElasticBeanstalkStack ebStack = new GoogleCustomSearchServiceElasticBeanstalkStack(app, "google-custom-search-service-elastic-beanstalk-stack", new GoogleCustomSearchServiceElasticBeanstalkStackProps 
        {
            ApplicationName = "GoogleCustomSearchService",
            //Vpc = DatabaseStack.Vpc,
            Env = new Amazon.CDK.Environment
            {
                Account = System.Environment.GetEnvironmentVariable("PROJECTS_AWS_DEFAULT_ACCOUNT", EnvironmentVariableTarget.User),
                Region = "us-east-1"
                //Region = System.Environment.GetEnvironmentVariable("PROJECTS_AWS_DEFAULT_REGION", EnvironmentVariableTarget.User)
            }
        });

        //ebStack.AddDependency(dbStack);
         
        //dbMigrationLambdaStack.AddDependency(ebStack);*/

        app.Synth();
    }
}
