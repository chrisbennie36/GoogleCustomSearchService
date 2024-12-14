using GoogleCustomSearchService.Api.WebApplication.Responses;

namespace GoogleCustomSearchService.Api.WebApplication.Extensions;

public static class GoogleCustomSearchResponseExtensions
{
    public static ApiResponse<GoogleCustomSearchResponse> ToSuccessResponse(this GoogleCustomSearchResponse response)
    {
        return new ApiResponse<GoogleCustomSearchResponse>(response);
    }
}
