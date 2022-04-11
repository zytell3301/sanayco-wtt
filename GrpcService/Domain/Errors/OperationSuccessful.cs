namespace GrpcService1.Domain.Errors;

public class OperationSuccessful : Status
{
    public OperationSuccessful(string message) : base(message)
    {
        /*
         * Status code 0 for successful call
         */
        StatusCode = 0;
    }
}