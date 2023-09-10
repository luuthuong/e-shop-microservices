using MediatR;

namespace Domain.Core;

public abstract record BaseRequest
{
    public bool AutoSave { get; init; } = false;
}