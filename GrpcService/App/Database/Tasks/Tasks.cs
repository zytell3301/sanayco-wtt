#region

using ErrorReporter;
using GrpcService1.App.Core.Tasks;
using GrpcService1.App.Database.Model;
using GrpcService1.Domain.Errors;

#endregion

namespace GrpcService1.App.Database.Tasks;

public class Tasks : IDatabase
{
    private readonly wttContext Connection;
    private readonly IErrorReporter ErrorReporter;
    private readonly InternalError InternalError;

    public class TasksDatabaseDependencies
    {
        public wttContext Connection;
        public IErrorReporter ErrorReporter;
        public InternalError InternalError;
    }

    public Tasks(TasksDatabaseDependencies dependencies)
    {
        Connection = dependencies.Connection;
        ErrorReporter = dependencies.ErrorReporter;
        InternalError = dependencies.InternalError;
    }

    public void RecordTask(Domain.Entities.Task task)
    {
        try
        {
            Connection.Tasks.Add(new Model.Task
            {
                Description = task.Description,
                Status = task.Status,
                Title = task.Title,
                ProjectId = task.ProjectId,
                WorkLocation = task.WorkLocation,
                CreatedAt = task.CreatedAt,
                UserId = task.UserId,
                Points = task.Points,
                StartTime = task.StartTime
            });
            Connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
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
            throw InternalError;
        }
    }

    public void EditTask(Domain.Entities.Task task)
    {
        try
        {
            var model = Connection.Tasks.First(t => t.Id == task.Id);
            model.Description = task.Description;
            model.Title = task.Title;
            model.WorkLocation = task.WorkLocation;
            model.Points = task.Points;
            model.StartTime = task.StartTime;
            UpdateTask(model);
            Connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
    }

    public void ChangeTaskStatus(Domain.Entities.Task task, string status)
    {
        try
        {
            var model = Connection.Tasks.First(t => t.Id == task.Id);
            model.Status = status;
            Connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
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
            throw InternalError;
        }
    }

    public List<Domain.Entities.Task> GetTaskRange(DateTime fromDate, DateTime toDate, int userId, int projectId,
        string workLocation)
    {
        var tasks = new List<Domain.Entities.Task>();
        try
        {
            foreach (var task in Connection.Tasks.Where(t =>
                             t.CreatedAt > fromDate && t.CreatedAt < toDate && t.UserId == userId &&
                             t.ProjectId == projectId && t.WorkLocation == workLocation)
                         .ToList())
                tasks.Add(new Domain.Entities.Task
                {
                    Id = task.Id,
                    Description = task.Description,
                    Points = task.Points,
                    Title = task.Title,
                    Status = task.Status,
                    ProjectId = task.ProjectId.Value,
                    StartTime = task.StartTime,
                    UserId = task.UserId.Value,
                    WorkLocation = task.WorkLocation,
                    CreatedAt = task.CreatedAt.Value
                });

            return tasks;
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
    }

    public List<Domain.Entities.Task> GetUserTasks(DateTime fromDate, DateTime toDate, int userId)
    {
        try
        {
            var tasks = new List<Domain.Entities.Task>();
            foreach (var task in Connection.Tasks.Where(t => t.CreatedAt > fromDate).Where(t => t.CreatedAt < toDate)
                         .Where(t => t.UserId == userId).ToList())
                tasks.Add(ConvertModelToTask(task));

            return tasks;
        }
        catch (Exception e)
        {
            throw InternalError;
        }
    }

    private void UpdateTask(Model.Task task)
    {
        Connection.Tasks.Update(task);
    }

    private Domain.Entities.Task ConvertModelToTask(Model.Task model)
    {
        var task = new Domain.Entities.Task
        {
            Id = model.Id,
            Description = model.Description,
            Status = model.Status,
            Title = model.Title,
            WorkLocation = model.WorkLocation,
            Points = model.Points,
            StartTime = model.StartTime
        };
        switch (task.CreatedAt.HasValue)
        {
            case true:
                task.CreatedAt = model.CreatedAt.Value;
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