namespace ErrorReporter;

public class Reporter : IErrorReporter
{
    /*
     * This class is implemented for TESTING PURPOSE for now.
     * @TODO Complete implementation of error reporter
     */
    public void ReportException(Exception exception)
    {
        Console.WriteLine(exception.Message);
    }
}