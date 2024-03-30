using Core.Results;

namespace Core.Api;

public record ApiResponse<T>(
    Result Status,
    T Data = default
);
