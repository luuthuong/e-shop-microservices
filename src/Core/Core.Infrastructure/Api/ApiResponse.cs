using Core.Results;

namespace Core.Infrastructure.Api;

public record ApiResponse<T>(
    Result Status,
    T Data = default
);
