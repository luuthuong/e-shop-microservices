namespace Core;

public sealed class AuthorizeFailException(string message): ApplicationException(message: message);

public sealed class AuthenticateFailedException(string message): ApplicationException(message: message);