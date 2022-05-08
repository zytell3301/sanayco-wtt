#region

using ErrorReporter;
using GrpcService1.App.Core.Presentation;
using GrpcService1.App.Database.Model;
using GrpcService1.Domain.Errors;
using User = GrpcService1.Domain.Entities.User;

#endregion

namespace GrpcService1.App.Database.Presentations;

public class Presentations : IDatabase
{
    private readonly wttContext Connection;
    private readonly IErrorReporter ErrorReporter;

    public Presentations(wttContext connection, IErrorReporter errorReporter)
    {
        Connection = connection;
        ErrorReporter = errorReporter;
    }

    public void RecordPresentation(User user)
    {
        try
        {
            Connection.Presentations.Add(new Presentation
            {
                UserId = user.Id,
                Start = DateTime.Now
            });
            Connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw new InternalError("");
        }
    }

    public void RecordPresentationEnd(User user)
    {
        try
        {
            Connection.Presentations.OrderBy(p => p.Id).Last(p => p.UserId == user.Id).End = DateTime.Now;
            Connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw new InternalError("");
        }
    }

    public DateTime GetPresentationTime(User user)
    {
        try
        {
            return (DateTime) Connection.Presentations.Last(p => p.UserId == user.Id).Start;
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw new InternalError("");
        }
    }

    public List<Domain.Entities.Presentation> GetPresentationsRange(DateTime fromDate, DateTime toDate, int userId)
    {
        var presentations = new List<Domain.Entities.Presentation>();
        try
        {
            foreach (var presentation in Connection.Presentations.Where(p => p.Start > fromDate)
                         .Where(p => p.End < toDate)
                         .Where(p => p.UserId == userId).ToList())
            {
                presentations.Add(new Domain.Entities.Presentation()
                {
                    Id = presentation.Id,
                    End = presentation.End,
                    Start = presentation.Start,
                    UserId = presentation.UserId.Value,
                });
            }

            return presentations;
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw new InternalError("");
        }
    }
}