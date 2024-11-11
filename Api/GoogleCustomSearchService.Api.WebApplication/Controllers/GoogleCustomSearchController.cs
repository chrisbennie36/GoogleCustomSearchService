using AutoMapper;
using GoogleCustomSearchService.Api.Domain.Queries;
using GoogleCustomSearchService.Api.Domain.Results;
using GoogleCustomSearchService.Api.WebApplication.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace GoogleCustomSearchService.Api.WebApplication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GoogleCustomSearchController : ControllerBase
{
    private readonly ISender sender;
    private readonly IMapper mapper;

    public GoogleCustomSearchController(ISender sender, IMapper mapper)
    {
        this.sender = sender;
        this.mapper = mapper;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetResults([FromBody] GoogleCustomSearchDto googleCustomSearchDto/*, IMemoryCache cache*/)
    {        
        var result = await sender.Send(mapper.Map<GetGoogleResultsQuery>(googleCustomSearchDto));

        /*if(result != null)
        {
            MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(2));

            foreach(Item item in result.Items)
            {
                cache.Set<string>(item.Link, item.Link, cacheOptions);
            }
        }*/

        return Ok(result);
    }
}
