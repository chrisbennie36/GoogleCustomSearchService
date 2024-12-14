using System.Diagnostics;
using GoogleCustomSearchService.Api.Domain.Results;
using Microsoft.AspNetCore.Mvc;

namespace GoogleCustomSearchService.Api.WebApplication.Responses;

public class GoogleCustomSearchResponse 
{
    public ProblemDetails? ProblemDetails { get; set; }
    public IEnumerable<Item> Items { get; set; } = new List<Item>();

    public static GoogleCustomSearchResponse FromProblemDetails(HttpContext httpContext, string errorMessage)
    {
        string traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

            ProblemDetails problemDetails = new ProblemDetails
            {
                Detail = errorMessage,
                Status = 400,
                Extensions = new Dictionary<string, object?> 
                {
                    { "traceId", traceId }
                }
            };

            return new GoogleCustomSearchResponse
            {
                ProblemDetails = problemDetails
            };
    }
}
