namespace Core.Results;

public sealed record Error(string Code, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
}


public class Result
{
    public bool IsSuccess { get; }
    public Error Error { get; }

    public bool IsFailure => !IsSuccess;
    private Result(bool isSuccess, Error error)
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