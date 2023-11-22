namespace Core.Exception;

public class DomainLogicException : System.Exception
{
    public DomainLogicException(){}
    public DomainLogicException(string message): base(message){}
}