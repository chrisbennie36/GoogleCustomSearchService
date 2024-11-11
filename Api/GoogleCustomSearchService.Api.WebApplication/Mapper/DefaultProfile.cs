using AutoMapper;
using GoogleCustomSearchService.Api.Domain.Queries;
using GoogleCustomSearchService.Api.WebApplication.Dtos;

namespace GoogleCustomSearchService.Api.WebApplication.Mapper;

public class DefaultProfile : Profile
{
    public DefaultProfile()
    {
        MapDtosToQueries();
    }

    private void MapDtosToQueries()
    {
        CreateMap<GoogleCustomSearchDto, GetGoogleResultsQuery>();
    }
}
