using GrpcService1.App.Core.Presentation;
using GrpcService1.Domain.Entities;
using GrpcService1.Domain.Errors;
using ErrorReporter;

namespace GrpcService1.App.Database.Presentations;

public class Presentations : IDatabase
{
    private App.Database.Model.wttContext Connection;
    private ErrorReporter.IErrorReporter ErrorReporter;

    public Presentations(App.Database.Model.wttContext connection, IErrorReporter errorReporter)
    {
        Connection = connection;
        ErrorReporter = errorReporter;
    }

    public void RecordPresentation(User user)
    {
        try
        {
            Connection.Presentations.Add(new App.Database.Model.Presentation()
            {
                UserId = user.Id,
                Start = DateTime.Now,
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
}