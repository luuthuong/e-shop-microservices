using FluentValidation;
using MediatR;

namespace Core.Mediator;

public class ValidatorBehavior<TRequest, TResponse>: IPipelineBehavior<TRequest, TResponse> where TRequest: notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidatorBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any()) 
            return await next();
        var context = new ValidationContext<TRequest>(request);
        var validationResults =
            await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        var failures = validationResults.SelectMany(x => x.Errors).Where(f => f is not null).ToList();
        if (failures.Count != 0)
            throw new FluentValidation.ValidationException(failures);
        return await next();
    }
}