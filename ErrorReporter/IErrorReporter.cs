namespace ErrorReporter;

public interface IErrorReporter
{
    public void ReportException(Exception exception);
}