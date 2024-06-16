using Core.Results;

namespace Core.Integration;

public record IntegrationHttpResponse<T>(bool IsSuccess, T? Data, Error Error) : Result<T>(IsSuccess, Data, Error) where T: notnull
{
    
}

public record IntegrationHttpResponse(bool IsSuccess, object? Data, Error Error) : Result<object>(IsSuccess, Data, Error) 
{
    
}