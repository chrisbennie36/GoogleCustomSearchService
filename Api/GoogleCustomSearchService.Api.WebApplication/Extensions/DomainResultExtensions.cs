using GoogleCustomSearchService.Api.Domain.Results;
using Microsoft.AspNetCore.Mvc;

namespace GoogleCustomSearchService.Api.WebApplication.Extensions;

public static class DomainResultExtensions
{
    public static ActionResult ToActionResult<T>(this DomainResult<T> domainResult)
    {
        return MapResponseModelDomainResult<T>(domainResult);
    }

    private static ActionResult MapResponseModelDomainResult<T>(DomainResult<T> domainResult)
    {
        switch(domainResult.status)
        {
            case ResponseStatus.Success:
                return new OkObjectResult(domainResult.resultModel);
            case ResponseStatus.NotFound:
                return new NotFoundResult();
            default:
                return new BadRequestObjectResult(domainResult.errorMessage);
        }
    }
}
