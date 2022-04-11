namespace GrpcService1.ErrorReporter;

public interface IErrorReporter
{
    public void ReportException(Exception exception);
}