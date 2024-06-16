namespace Core.Results;

public sealed record class Error
{
    public Error()
    {
        
    }
    public Error(string code, string description)
    {
        Code = code;
        Description = description;
    }

    public string Code { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;

    public static readonly Error None = new();
}

public record class Result
{
    public bool IsSuccess { get; }
    public Error Error { get; set; }

    public bool IsFailure => !IsSuccess;

    protected Result(bool isSuccess, Error error)
    {
        if (
            isSuccess && error != Error.None
            || !isSuccess && error == Error.None
        )
        {
            throw new ArgumentException("Invalid Error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);
}

public record class Result<TData>(bool IsSuccess, TData? Data, Error Error) : Result(IsSuccess, Error)
{
    public static Result<TData> Success(TData data) => new(true, data, Error.None);
    public new static Result<TData> Failure(Error error) => new(false, default, error);
}