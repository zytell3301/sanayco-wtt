﻿using GrpcService1.App.Core.Tasks;
using GrpcService1.Domain.Errors;
using ErrorReporter;

namespace GrpcService1.App.Database.Tasks;

public class Tasks : IDatabase
{
    private Connection Connection;
    private IErrorReporter ErrorReporter;

    public Tasks(Connection connection, IErrorReporter errorReporter)
    {
        Connection = connection;
        ErrorReporter = errorReporter;
    }

    public void RecordTask(Domain.Entities.Task task)
    {
        try
        {
            Connection.Tasks.Add(new Model()
            {
                // CreatedAt is not initialized because it will be evaluated in database
                Description = task.Description,
                Status = task.Status,
                Title = task.Title,
                EndTime = task.EndTime.Second,
                ProjectId = task.ProjectId,
                WorkLocation = task.WorkLocation,
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
            Connection.Remove(new Model()
            {
                // Since id is pk of tasks entity ,any other field can be discarded
                Id = task.Id,
            });
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
            UpdateTask(new Model()
            {
                Id = task.Id,
                Description = task.Description,
                Title = task.Title,
                EndTime = task.EndTime.Second,
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

    private void UpdateTask(Model task)
    {
        Connection.Tasks.Update(task);
    }

    public void ChangeTaskStatus(Domain.Entities.Task task, string status)
    {
        try
        {
            UpdateTask(new Model()
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
        var task = Connection.Tasks.First(p => p.Id == taskId);
        return ConvertModelToTask(task);
    }

    private Domain.Entities.Task ConvertModelToTask(Model model)
    {
        var task = new Domain.Entities.Task()
        {
            Id = model.Id,
            Description = model.Description,
            Status = model.Status,
            Title = model.Title,
            EndTime = DateTime.UnixEpoch.AddSeconds(model.EndTime),
            ProjectId = model.ProjectId,
            UserId = model.UserId,
            WorkLocation = model.WorkLocation,
        };
        switch (task.CreatedAt.HasValue)
        {
            case true:
                task.CreatedAt = DateTime.UnixEpoch.AddSeconds(model.CreatedAt.Value);
                break;
        }

        return task;
    }
}