using GrpcService1.App.Core.OffTime;
using GrpcService1.Domain.Entities;

namespace GrpcService1.Database.OffTime;

public class OffTime : IDatabase
{
    private Database.OffTime.Connection Connection;
    private ErrorReporter.IErrorReporter ErrorReporter;

    public OffTime(Database.OffTime.Connection connection, ErrorReporter.IErrorReporter errorReporter)
    {
        Connection = connection;
        ErrorReporter = errorReporter;
    }

    public void RecordOffTime(Domain.Entities.OffTime offTime)
    {
        throw new NotImplementedException();
    }

    public Domain.Entities.OffTime[] GetOffTimeHistory(User user, DateTime @from, DateTime to)
    {
        throw new NotImplementedException();
    }

    public void ChangeOffTimeStatus(Domain.Entities.OffTime offTime, string status)
    {
        throw new NotImplementedException();
    }
}