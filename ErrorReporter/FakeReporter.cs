namespace ErrorReporter;

public class FakeReporter : IErrorReporter
{
    /*
     * THIS CLASS IS ONLY IMPLEMENTED FOR TESTING PURPOSES.
     * If the application is moving to production, the original
     * reporter class must be uses or you will lose the control
     * over all errors that are being happened in application.
     */
    public void ReportException(Exception exception)
    {
        Console.WriteLine(exception.Message);
    }
}