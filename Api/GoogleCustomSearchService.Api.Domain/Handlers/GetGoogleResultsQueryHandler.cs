using GoogleCustomSearchService.Api.Domain.Clients.Interfaces;
using GoogleCustomSearchService.Api.Domain.Queries;
using GoogleCustomSearchService.Api.Domain.Results;
using MediatR;

namespace GoogleCustomSearchService.Api.Domain.Handlers;

public class GetGoogleResultsQueryHandler : IRequestHandler<GetGoogleResultsQuery, GoogleCustomSearchResult?>
{
    private IGoogleCustomSearchClient googleCustomSearchClient;

    public GetGoogleResultsQueryHandler(IGoogleCustomSearchClient googleCustomSearchClient)
    {
        this.googleCustomSearchClient = googleCustomSearchClient;
    }

    public async Task<GoogleCustomSearchResult?> Handle(GetGoogleResultsQuery request, CancellationToken cancellationToken)
    {
        GoogleCustomSearchResult? result = await googleCustomSearchClient.GetResults(request.QueryString, request.PaginationToken);

        return result;
    }

}
