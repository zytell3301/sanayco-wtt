using GrpcService1.App.Core.OffTime;
using GrpcService1.Domain.Entities;
using GrpcService1.Domain.Errors;

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
        try
        {
            Connection.OffTimes.Add(new Domain.Entities.OffTime()
            {
                Description = offTime.Description,
                Status = offTime.Status,
                FromDate = offTime.FromDate,
                ToDate = offTime.ToDate,
                UserId = offTime.UserId,
            });
            Connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw new InternalError("");
        }
    }

    public List<Domain.Entities.OffTime> GetOffTimeHistory(User user, DateTime @from, DateTime to)
    {
        List<Domain.Entities.OffTime> offTimes = new List<Domain.Entities.OffTime>();
        try
        {
            var enumerator = Connection.OffTimes.Where(o => o.UserId == user.Id).Where(o => o.CreatedAt > from)
                .Where(o => o.CreatedAt < to).GetEnumerator();
            while (enumerator.MoveNext())
            {
                offTimes.Add(enumerator.Current);
            }
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw new InternalError("");
        }

        return offTimes;
    }

    public void ChangeOffTimeStatus(Domain.Entities.OffTime offTime, string status)
    {
        throw new NotImplementedException();
    }
}