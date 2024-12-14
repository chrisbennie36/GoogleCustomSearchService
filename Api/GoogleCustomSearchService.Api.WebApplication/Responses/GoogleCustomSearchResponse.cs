using GoogleCustomSearchService.Api.Domain.Results;
using Microsoft.AspNetCore.Mvc;

namespace GoogleCustomSearchService.Api.WebApplication.Responses;

public class GoogleCustomSearchResponse 
{
    public ProblemDetails? ProblemDetails { get; set; }
    public IEnumerable<Item> Items { get; set; } = new List<Item>();
}
