using GrpcService1.App.Core.OffTime;
using GrpcService1.Domain.Entities;
using GrpcService1.Domain.Errors;

namespace GrpcService1.App.Database.OffTime;

public class OffTimes : IDatabase
{
    private App.Database.Model.wttContext Connection;
    private ErrorReporter.IErrorReporter ErrorReporter;

    public OffTimes(App.Database.Model.wttContext connection, ErrorReporter.IErrorReporter errorReporter)
    {
        Connection = connection;
        ErrorReporter = errorReporter;
    }

    public void RecordOffTime(Domain.Entities.OffTime offTime)
    {
        try
        {
            Connection.OffTimes.Add(new App.Database.Model.OffTime()
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
                .Where(o => o.CreatedAt < to).ToList();
            foreach (var offTime in enumerator)
            {
                offTimes.Add(ConvertModelToOffTime(offTime));
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
        try
        {
            var record = Connection.OffTimes.First(o => o.Id == offTime.Id);
            record.Status = status;
            Connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw new InternalError("");
        }
    }

    public void DeleteOffTime(Domain.Entities.OffTime offTime)
    {
        try
        {
            Model.OffTime model = Connection.OffTimes.First(o => o.Id == offTime.Id);
            Connection.OffTimes.Remove(model);
            Connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw new InternalError("");
        }
    }

    private Domain.Entities.OffTime ConvertModelToOffTime(Database.Model.OffTime model)
    {
        // Database offers the feature of null foreign key value but we always supply values to user_id,from_date and to_date fields.
        // So it is not needed to check for null reference
        var offTime = new Domain.Entities.OffTime()
        {
            Id = model.Id,
            Status = model.Status,
            Description = model.Description,
            CreatedAt = model.CreatedAt,
            UserId = model.UserId.Value,
            FromDate = model.FromDate.Value,
            ToDate = model.ToDate.Value,
        };

        return offTime;
    }
}