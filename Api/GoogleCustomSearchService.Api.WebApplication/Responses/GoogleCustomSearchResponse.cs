using GoogleCustomSearchService.Api.Domain.Results;

namespace GoogleCustomSearchService.Api.WebApplication.Responses;

public class GoogleCustomSearchResponse
{
    public IEnumerable<Item> Items { get; set; } = new List<Item>();
}
