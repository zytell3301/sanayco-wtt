namespace GrpcService1.Domain.Errors;

public class OffTimeRestrictionExceeded : Status
{
    public OffTimeRestrictionExceeded(string message) : base(message)
    {
        /*
         * Status code 0 for successful call
         */
        StatusCode = 2;
    }
}