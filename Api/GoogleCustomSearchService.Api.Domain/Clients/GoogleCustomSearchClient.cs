using GoogleCustomSearchService.Api.Domain.Clients.Interfaces;
using GoogleCustomSearchService.Api.Domain.Results;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using Serilog;

namespace GoogleCustomSearchService.Api.Domain.Clients;

public class GoogleCustomSearchClient : IGoogleCustomSearchClient
{
    private readonly IRestClient client;
    private readonly IConfiguration configuration;

    public GoogleCustomSearchClient(IConfiguration configuration)
    {
        this.configuration = configuration;
        client = new RestClient();
    }

    public async Task<GoogleCustomSearchResult?> GetResults(string queryParams, int paginationToken = 0)
    {
        string apiKey = configuration["GoogleCustomSearchEngine:ApiKey"] ?? string.Empty;
        string identifier = configuration["GoogleCustomSearchEngine:Identifier"] ?? string.Empty;

        //ToDo: Handle within a ConfigurationHelper
        if(string.IsNullOrWhiteSpace(apiKey))
        {
            Log.Error("GoogleCustomSearchEngine API Key must be configured");
            return null;
        }

        if(string.IsNullOrWhiteSpace(identifier))
        {
            Log.Error("GoogleCustomSearchEngine Identifier must be configured");
            return null;
        }

        string url = $"https://www.googleapis.com/customsearch/v1?key={apiKey}&cx={identifier}&q={queryParams}";

        if(paginationToken > 0)
        {
            url += $"&start={paginationToken}";
        }

        Log.Warning($"Final URL is: {url}");

        RestRequest request = new RestRequest(url);

        try
        {
            var response = await client.GetAsync(request);

            if(response == null || string.IsNullOrWhiteSpace(response.Content))
            {
                Log.Error("Response from REST API for getting Google results is null");
                return null;
            }

            if(!response.IsSuccessful)
            {
                Log.Error($"Error when calling the Google Custom Search API, error code: {response.StatusCode}, error message: {response.ErrorMessage}, error exception: {response.ErrorException}");
                return null;
            }

            try
            {
                GoogleCustomSearchResult? result = JsonConvert.DeserializeObject<GoogleCustomSearchResult>(response.Content);
                Log.Warning($"Returning search results with count: {result.Items.Count()}");
                return result;
            }
            catch(Exception e)
            {
                Log.Error($"Error when deserializating the Google Custom Search results, exception message: {e.Message}");
                return null;
            }
        }
        catch(Exception e)
        {
            Log.Error($"Error when calling {url}, error message: {e.Message}");
            return null;
        }
    }
}
