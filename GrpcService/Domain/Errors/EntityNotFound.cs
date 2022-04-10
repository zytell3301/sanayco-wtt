namespace GrpcService1.Domain.Errors;

public class EntityNotFound : Status
{
    public EntityNotFound(string message) : base(message)
    {
        StatusCode = 3;
    }
}