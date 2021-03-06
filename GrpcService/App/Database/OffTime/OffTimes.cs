#region

using ErrorReporter;
using GrpcService1.App.Core.OffTime;
using GrpcService1.App.Database.Model;
using GrpcService1.Domain.Errors;
using User = GrpcService1.Domain.Entities.User;

#endregion

namespace GrpcService1.App.Database.OffTime;

public class OffTimes : IDatabase
{
    private readonly wttContext Connection;
    private readonly IErrorReporter ErrorReporter;
    private readonly InternalError InternalError;

    public OffTimes(OffTimesDatabaseDependencies dependencies)
    {
        Connection = dependencies.Connection;
        ErrorReporter = dependencies.ErrorReporter;
        InternalError = dependencies.InternalError;
    }

    public void RecordOffTime(Domain.Entities.OffTime offTime)
    {
        try
        {
            Connection.OffTimes.Add(new Model.OffTime
            {
                Description = offTime.Description,
                Status = offTime.Status,
                FromDate = offTime.FromDate,
                ToDate = offTime.ToDate,
                UserId = offTime.UserId,
                CreatedAt = offTime.CreatedAt
            });
            Connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
    }

    public List<Domain.Entities.OffTime> GetOffTimeHistory(User user, DateTime from, DateTime to)
    {
        var offTimes = new List<Domain.Entities.OffTime>();
        try
        {
            var enumerator = Connection.OffTimes.Where(o => o.UserId == user.Id).Where(o => o.CreatedAt > from)
                .Where(o => o.CreatedAt < to).ToList();
            foreach (var offTime in enumerator) offTimes.Add(ConvertModelToOffTime(offTime));
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
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
            throw InternalError;
        }
    }

    public void DeleteOffTime(Domain.Entities.OffTime offTime)
    {
        try
        {
            var model = Connection.OffTimes.First(o => o.Id == offTime.Id);
            Connection.OffTimes.Remove(model);
            Connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
    }

    public void EditOffTime(Domain.Entities.OffTime offTime)
    {
        try
        {
            var model = Connection.OffTimes.First(o => o.Id == offTime.Id);
            model.FromDate = offTime.FromDate;
            model.ToDate = offTime.ToDate;
            model.Description = offTime.Description;
            Connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
    }

    public Domain.Entities.OffTime GetOffTime(Domain.Entities.OffTime offTime)
    {
        try
        {
            return ConvertModelToOffTime(Connection.OffTimes.First(o => o.Id == offTime.Id));
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
    }

    public List<Domain.Entities.OffTime> GetOffTimeListRange(DateTime fromDate, DateTime toDate, int userId)
    {
        var offtimes = new List<Domain.Entities.OffTime>();
        try
        {
            foreach (var offTime in Connection.OffTimes.Where(o => o.CreatedAt > fromDate && o.CreatedAt < toDate)
                         .ToList())
                offtimes.Add(new Domain.Entities.OffTime
                {
                    Id = offTime.Id,
                    Description = offTime.Description,
                    Status = offTime.Status,
                    CreatedAt = offTime.CreatedAt,
                    FromDate = offTime.FromDate.Value,
                    ToDate = offTime.ToDate.Value,
                    UserId = offTime.UserId.Value
                });

            return offtimes;
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
    }

    private Domain.Entities.OffTime ConvertModelToOffTime(Model.OffTime model)
    {
        // Database offers the feature of null foreign key value but we always supply values to user_id,from_date and to_date fields.
        // So it is not needed to check for null reference
        var offTime = new Domain.Entities.OffTime
        {
            Id = model.Id,
            Status = model.Status,
            Description = model.Description,
            CreatedAt = model.CreatedAt,
            UserId = model.UserId.Value,
            FromDate = model.FromDate.Value,
            ToDate = model.ToDate.Value
        };

        return offTime;
    }

    public class OffTimesDatabaseDependencies
    {
        public wttContext Connection;
        public IErrorReporter ErrorReporter;
        public InternalError InternalError;
    }
}