using Core.Results;

namespace Core.Infrastructure.Utils;

public static class ResultExtension
{
    public static T Match<T>(
        this Result result,
        Func<T> onSuccess,
        Func<Error, T> onFailure)
    {
        return result.IsSuccess ? onSuccess() : onFailure(result.Error);
    }
}