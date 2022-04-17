using GrpcService1.App.Core.Presentation;
using GrpcService1.Domain.Entities;
using GrpcService1.Domain.Errors;
using ErrorReporter;

namespace GrpcService1.App.Database.Presentations;

public class Presentations : IDatabase
{
    private Database.Presentations.Connection Connection;
    private ErrorReporter.IErrorReporter ErrorReporter;

    public Presentations(Database.Presentations.Connection connection, IErrorReporter errorReporter)
    {
        Connection = connection;
        ErrorReporter = errorReporter;
    }

    public void RecordPresentation(User user)
    {
        try
        {
            Connection.Presentations.Add(new Model()
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
            Connection.Presentations.Last(p => p.UserId == user.Id).End = DateTime.Now;
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