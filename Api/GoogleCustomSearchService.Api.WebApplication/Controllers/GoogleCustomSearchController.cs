using AutoMapper;
using GoogleCustomSearchService.Api.Domain.Queries;
using GoogleCustomSearchService.Api.WebApplication.Dtos;
using GoogleCustomSearchService.Api.WebApplication.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
    [ProducesResponseType<GoogleCustomSearchResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetResults([FromBody] GoogleCustomSearchDto googleCustomSearchDto/*, IMemoryCache cache*/)
    {        
        var result = await sender.Send(mapper.Map<GetGoogleResultsQuery>(googleCustomSearchDto));

        if(result == null)
        {
            return NotFound();
        }

        /*if(result != null)
        {
            MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(2));

            foreach(Item item in result.Items)
            {
                cache.Set<string>(item.Link, item.Link, cacheOptions);
            }
        }*/

        return Ok(mapper.Map<GoogleCustomSearchResponse>(result));
    }
}
