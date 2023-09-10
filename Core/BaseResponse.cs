namespace Domain.Core;

public abstract record BaseResponse<T>
{
    public bool Success { get; init; }
    public string ErrorMsg { get; set; }
    public T Data { get; init; }
}