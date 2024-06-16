namespace Core.Results;

public sealed record Error(string Code, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
}

public class Result
{
    public bool IsSuccess { get; }
    public Error Error;

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

public class Result<TData>(bool isSuccess, TData Data, Error error) : Result(isSuccess, error)
{
    public TData Data => Data;
    public static Result<TData> Success(TData data) => new(true, data, Error.None);
}