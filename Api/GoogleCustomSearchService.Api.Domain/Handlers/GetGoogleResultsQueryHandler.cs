using GoogleCustomSearchService.Api.Domain.Clients.Interfaces;
using GoogleCustomSearchService.Api.Domain.Queries;
using GoogleCustomSearchService.Api.Domain.Results;
using MediatR;

namespace GoogleCustomSearchService.Api.Domain.Handlers;

public class GetGoogleResultsQueryHandler : IRequestHandler<GetGoogleResultsQuery, DomainResult<GoogleCustomSearchResult>>
{
    private IGoogleCustomSearchClient googleCustomSearchClient;

    public GetGoogleResultsQueryHandler(IGoogleCustomSearchClient googleCustomSearchClient)
    {
        this.googleCustomSearchClient = googleCustomSearchClient;
    }

    public async Task<DomainResult<GoogleCustomSearchResult>> Handle(GetGoogleResultsQuery request, CancellationToken cancellationToken)
    {                
        GoogleCustomSearchResult? result;
        try
        {
            result = await googleCustomSearchClient.GetResults(request.QueryString, request.PaginationToken);
        }
        catch(Exception e)
        {
            return new DomainResult<GoogleCustomSearchResult>(ResponseStatus.Error, null, e.Message);
        }

        if(result == null) 
        {
            return new DomainResult<GoogleCustomSearchResult>(ResponseStatus.NotFound, null);
        }

        return new DomainResult<GoogleCustomSearchResult>(ResponseStatus.Success, result);
    }

}
