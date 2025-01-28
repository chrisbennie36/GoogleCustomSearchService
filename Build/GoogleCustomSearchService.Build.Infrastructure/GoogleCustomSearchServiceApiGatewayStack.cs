using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Constructs;
using Utilities.RecipeRandomizer.Infrastructure.CDK.Constants.ApiGateway;

namespace GoogleCustomSearchService.Build.Infrastructure;

public class GoogleCustomSearchServiceApiGatewayProps : StackProps
{
    public required string BaseUrl { get; set; }
    public string? RestApiId { get; set; }
    public string? RootResourceId { get; set; }
}

public class GoogleCustomSearchServiceApiGatewayStack : Stack
{
    public GoogleCustomSearchServiceApiGatewayStack(Construct scope, string id, GoogleCustomSearchServiceApiGatewayProps props) : base(scope, id)
    {
        IRestApi restApi;

        if(!string.IsNullOrWhiteSpace(props.RestApiId) && !string.IsNullOrWhiteSpace(props.RootResourceId))
        {
            restApi = GetExistingRestApi(this, ApiGatewayExportKeys.RecipeRandomizerApiGatewayRestApiId, ApiGatewayExportKeys.RecipeRandomizerApiGatewayRootResourceId);
        }
        else
        {
            restApi = new RestApi(this, "google-custom-search-api-gateway");
        }

        AddRestApiResourceProxy(restApi, "GoogleCustomSearch", props.BaseUrl);
    }

    private IRestApi GetExistingRestApi(Construct scope, string restApiIdImportKey, string rootResourceIdKey)
    {
        string restApiId = Fn.ImportValue(restApiIdImportKey);
        string rootResourceId = Fn.ImportValue(rootResourceIdKey);

        return RestApi.FromRestApiAttributes(scope, "recipe-randomizer-api-gateway", new RestApiAttributes 
        {
            RootResourceId = rootResourceId,
            RestApiId = restApiId
        });
    }

    private void AddRestApiResourceProxy(IRestApi restApi, string resourceName, string apiBaseUrl)
    {
        restApi.Root.AddResource(resourceName).AddProxy(new ProxyResourceOptions 
        {
            DefaultIntegration = new Integration(new IntegrationProps {
                Type = IntegrationType.HTTP_PROXY,
                Uri = $"{apiBaseUrl}/api/{resourceName}/",
                IntegrationHttpMethod = "ANY" 
            })
        });
    } 
}
