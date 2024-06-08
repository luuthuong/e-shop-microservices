using Core.Results;

namespace Core.Api;

public record ApiResponse(Result Status);

public record ApiResponse<T>(
    Result Status,
    T? Data = default
) : ApiResponse(Status);