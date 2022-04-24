namespace GrpcService1.Domain.Errors;

public class AuthenticationFailed : Status
{
    public AuthenticationFailed(string message) : base(message)
    {
    }
}