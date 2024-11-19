using AutoMapper;
using GoogleCustomSearchService.Api.Domain.Queries;
using GoogleCustomSearchService.Api.Domain.Results;
using GoogleCustomSearchService.Api.WebApplication.Dtos;
using GoogleCustomSearchService.Api.WebApplication.Responses;

namespace GoogleCustomSearchService.Api.WebApplication.Mapper;

public class DefaultProfile : Profile
{
    public DefaultProfile()
    {
        MapDtosToQueries();
        MapResultsToResponses();
    }

    private void MapDtosToQueries()
    {
        CreateMap<GoogleCustomSearchDto, GetGoogleResultsQuery>();
    }

    private void MapResultsToResponses()
    {
        CreateMap<GoogleCustomSearchResult, GoogleCustomSearchResponse>();
    }
}
