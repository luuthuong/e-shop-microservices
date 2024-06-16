using Core.Results;

namespace Identity.Domains;

public static class AuthenticationError
{
    public static Error AuthenticateFailed => new("AuthenticatedFail", "Authenticate failed");
}