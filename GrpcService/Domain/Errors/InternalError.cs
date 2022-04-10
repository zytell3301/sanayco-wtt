namespace GrpcService1.Domain.Errors;

public class InternalError : Status
{
    public InternalError(string message) : base(message)
    {
        /*
         * Status code for internal error
         */
        StatusCode = 1;
    }
}