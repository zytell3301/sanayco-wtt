namespace GrpcService1.Domain.Errors;

/**
 * This is base class for all statuses that exists cross the application.
 * Every status represents an error message (If supplied by the creator of class).
 * StatusCodes must be unique cross the application so the caller can exactly distinct errors.
 * Status code 0 is reserved as successful call and 1 is reserved for internal error.
 */
public class Status : Exception
{
    protected string Message;
    protected int StatusCode;

    public Status(string message)
    {
        Message = message;
    }

    public int GetStatusCode()
    {
        return StatusCode;
    }

    public string GetMessage()
    {
        return Message;
    }
}