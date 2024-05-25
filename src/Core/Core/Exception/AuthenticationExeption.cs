namespace Core;

public sealed class AuthorizeFailException(string Message): ApplicationException(message: Message);

public sealed class AuthenticateFailedException(string Message): ApplicationException(message: Message);