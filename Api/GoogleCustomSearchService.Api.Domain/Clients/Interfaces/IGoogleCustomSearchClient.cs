using GoogleCustomSearchService.Api.Domain.Results;

namespace GoogleCustomSearchService.Api.Domain.Clients.Interfaces;

public interface IGoogleCustomSearchClient
{
        Task<GoogleCustomSearchResult?> GetResults(string queryParams, int paginationToken = 0);
}
