using GrpcService1.App.Core.Tasks;
using GrpcService1.Domain.Errors;
using GrpcService1.ErrorReporter;

namespace GrpcService1.Database.Tasks;

public class Tasks : IDatabase
{
    private Connection Connection;
    private IErrorReporter ErrorReporter;

    public Tasks(Connection connection)
    {
        Connection = connection;
    }

    public void RecordTask(Domain.Entities.Task task)
    {
        try
        {
            Connection.Tasks.Add(new Domain.Entities.Task()
            {
                // CreatedAt is not initialized because it will be evaluated in database
                Description = task.Description,
                Status = task.Status,
                Title = task.Title,
                EndTime = task.EndTime,
                ProjectId = task.ProjectId,
                WorkLocation = task.WorkLocation,
            });
            Connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw new InternalError("");
        }
    }

    public void DeleteTask(Domain.Entities.Task task)
    {
        throw new NotImplementedException();
    }

    public void EditTask(Domain.Entities.Task task)
    {
        throw new NotImplementedException();
    }

    public void ChangeTaskStatus(Domain.Entities.Task task, string status)
    {
        throw new NotImplementedException();
    }
}