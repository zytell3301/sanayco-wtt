namespace GrpcService1.Domain.Errors;

public class InvalidCredentials : Status
{
    public InvalidCredentials(string message) : base(message)
    {
    }
}