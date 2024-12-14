namespace GoogleCustomSearchService.Api.WebApplication.Responses;

public class ApiResponse<T> where T : class
{
    public ApiResponse(T result)
    {
        Success = true;
        ResponseModel = result;
        ErrorTraceId= string.Empty;
    }

    public ApiResponse(string errorTraceId)
    {
        Success = false;
        ResponseModel = null;
        ErrorTraceId = errorTraceId;
    }

    public bool Success { get; set; }
    public T? ResponseModel { get; set; }
    public string ErrorTraceId { get; set; } = string.Empty;
}
