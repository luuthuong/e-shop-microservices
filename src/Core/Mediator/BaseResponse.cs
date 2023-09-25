namespace Core.Mediator;

public  record BaseResponse<T>
{
    public bool Success { get; init; }
    public string ErrorMsg { get; set; }
    public T Data { get; init; }

    public static BaseResponse<T> Succeed(T data) => new()
    {
        Success = true,
        Data = data
    };
    
    public static BaseResponse<string> Succeed(string msg = "") => new()
    {
        Success = false,
        ErrorMsg = msg
    };
}