using Core.Results;

namespace Core.Infrastructure.Api;

public class ApiResponse<T> 
{
    public required Result Status { get; set; }
    public T? Data { get; set; }
}