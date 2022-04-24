namespace GrpcService1.Domain.Errors;

public class AuthorizationFailed : Status
{
    public AuthorizationFailed(string message) : base(message)
    {
    }
}