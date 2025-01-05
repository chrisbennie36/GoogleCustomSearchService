using GoogleCustomSearchService.Api.Domain.Results;
using MediatR;
using Utilities.ResultPattern;

namespace GoogleCustomSearchService.Api.Domain.Queries;

public class GetGoogleResultsQuery : IRequest<DomainResult<GoogleCustomSearchResult>>
{
    public string QueryString { get; set; } = string.Empty;
    public int PaginationToken { get; set; }
}
