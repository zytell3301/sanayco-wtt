using GrpcService1.App.Core.Tasks;
using GrpcService1.Domain.Errors;
using ErrorReporter;

namespace GrpcService1.App.Database.Tasks;

public class Tasks : IDatabase
{
    private Database.Model.wttContext Connection;
    private IErrorReporter ErrorReporter;

    public Tasks(Database.Model.wttContext connection, IErrorReporter errorReporter)
    {
        Connection = connection;
        ErrorReporter = errorReporter;
    }

    public void RecordTask(Domain.Entities.Task task)
    {
        try
        {
            Connection.Tasks.Add(new Database.Model.Task()
            {
                // CreatedAt is not initialized because it will be evaluated in database
                Description = task.Description,
                Status = task.Status,
                Title = task.Title,
                EndTime = task.EndTime,
                ProjectId = task.ProjectId,
                WorkLocation = task.WorkLocation,
                CreatedAt = task.CreatedAt,
                UserId = task.UserId,
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
        try
        {
            var model = Connection.Tasks.First(t => t.Id == task.Id);
            Connection.Remove(model);
            Connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw new InternalError("");
        }
    }

    public void EditTask(Domain.Entities.Task task)
    {
        try
        {
            var model = Connection.Tasks.First(t => t.Id == task.Id);
            model.Description = task.Description;
            model.Title = task.Title;
            model.EndTime = task.EndTime;
            model.WorkLocation = task.WorkLocation;
            UpdateTask(model);
            Connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw new InternalError("");
        }
    }

    private void UpdateTask(Database.Model.Task task)
    {
        Connection.Tasks.Update(task);
    }

    public void ChangeTaskStatus(Domain.Entities.Task task, string status)
    {
        try
        {
            UpdateTask(new Database.Model.Task()
            {
                Id = task.Id,
                Status = task.Status,
            });
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw new InternalError("");
        }
    }

    public Domain.Entities.Task GetTask(int taskId)
    {
        try
        {
            var task = Connection.Tasks.First(p => p.Id == taskId);
            return ConvertModelToTask(task);
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw new InternalError("");
        }
    }

    private Domain.Entities.Task ConvertModelToTask(Database.Model.Task model)
    {
        var task = new Domain.Entities.Task()
        {
            Id = model.Id,
            Description = model.Description,
            Status = model.Status,
            Title = model.Title,
            WorkLocation = model.WorkLocation,
        };
        switch (task.CreatedAt.HasValue)
        {
            case true:
                task.CreatedAt = model.CreatedAt.Value;
                break;
        }

        switch (model.EndTime.HasValue)
        {
            case true:
                task.EndTime = model.EndTime.Value;
                break;
        }

        switch (model.ProjectId.HasValue)
        {
            case true:
                task.ProjectId = model.ProjectId.Value;
                break;
        }

        // Since it MAY be required in future updates that a task can be added without an assignee, we must check for null value
        switch (model.UserId.HasValue)
        {
            case true:
                task.UserId = model.UserId.Value;
                break;
        }

        return task;
    }
}