using AutoMapper;
using GoogleCustomSearchService.Api.Domain.Queries;
using GoogleCustomSearchService.Api.Domain.Results;
using GoogleCustomSearchService.Api.WebApplication.Dtos;
using GoogleCustomSearchService.Api.WebApplication.Extensions;
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
    public async Task<ActionResult<GoogleCustomSearchResponse>> GetResults([FromBody] GoogleCustomSearchDto googleCustomSearchDto)
    {   
        var result = await sender.Send(mapper.Map<GetGoogleResultsQuery>(googleCustomSearchDto));

        if(result.status == ResponseStatus.Success)
        {
            return Ok(mapper.Map<GoogleCustomSearchResponse>(result.resultModel));
        }

        return result.ToActionResult();   
    }
}
