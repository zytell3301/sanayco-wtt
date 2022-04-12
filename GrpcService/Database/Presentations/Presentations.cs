using GrpcService1.App.Core.Presentation;
using GrpcService1.Domain.Entities;
using GrpcService1.ErrorReporter;

namespace GrpcService1.Database.Presentations;

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
        throw new NotImplementedException();
    }

    public void RecordPresentationEnd(User user)
    {
        throw new NotImplementedException();
    }

    public DateTime GetPresentationTime(User user)
    {
        throw new NotImplementedException();
    }
}