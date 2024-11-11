using GoogleCustomSearchService.Api.Domain.Results;
using MediatR;

namespace GoogleCustomSearchService.Api.Domain.Queries;

public class GetGoogleResultsQuery : IRequest<GoogleCustomSearchResult?>
{
    public string QueryString { get; set; } = string.Empty;
    public int PaginationToken { get; set; }
}
