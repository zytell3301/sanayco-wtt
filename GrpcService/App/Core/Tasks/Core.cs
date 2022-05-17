#region

using GrpcService1.Domain.Errors;

#endregion

namespace GrpcService1.App.Core.Tasks;

public class Core
{
    private readonly IDatabase Database;
    private readonly InternalError InternalError;
    private readonly OperationSuccessful OperationSuccessful;
    public string ApprovedTaskCode;
    public string RejectedTaskCode;
    public string WaitingTaskCode;

    public Core(TasksCoreDependencies dependencies, TasksCoreConfigs configs)
    {
        OperationSuccessful = new OperationSuccessful(configs.OperationSuccessfulMessage);
        InternalError = new InternalError(configs.InternalErrorMessage);
        ApprovedTaskCode = configs.ApprovedTaskCode;
        WaitingTaskCode = configs.WaitingTaskCode;
        RejectedTaskCode = configs.UnApprovedTaskCode;

        Database = dependencies.Database;
    }

    public bool CheckTaskOwnership(int taskId, int userId)
    {
        var task = Database.GetTask(taskId);
        return task.UserId == userId;
    }

    /*
     * This method records the given task. 
     */
    public void RecordTask(Domain.Entities.Task task)
    {
        try
        {
            task.Status = WaitingTaskCode;
            task.CreatedAt = DateTime.Now;
            Database.RecordTask(task);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public void DeleteTask(Domain.Entities.Task task)
    {
        try
        {
            Database.DeleteTask(task);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public void EditTask(Domain.Entities.Task task)
    {
        try
        {
            Database.EditTask(task);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public void ApproveTask(Domain.Entities.Task task)
    {
        try
        {
            Database.ChangeTaskStatus(task, ApprovedTaskCode);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public void RejectTask(Domain.Entities.Task task)
    {
        try
        {
            Database.ChangeTaskStatus(task, RejectedTaskCode);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public void SetTaskWaiting(Domain.Entities.Task task)
    {
        try
        {
            Database.ChangeTaskStatus(task, WaitingTaskCode);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public Domain.Entities.Task GetTask(Domain.Entities.Task task)
    {
        try
        {
            return Database.GetTask(task.Id);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public List<Domain.Entities.Task> GetTaskRange(DateTime fromDate, DateTime toDate, int userId, int projectId,
        string workLocation)
    {
        try
        {
            return Database.GetTaskRange(fromDate, toDate, userId, projectId, workLocation);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public List<Domain.Entities.Task> GetUserTasks(DateTime fromDate, DateTime toDate, int userId)
    {
        try
        {
            return Database.GetUserTasks(fromDate, toDate, userId);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public class TasksCoreConfigs
    {
        public string ApprovedTaskCode;
        public string InternalErrorMessage;
        public string OperationSuccessfulMessage;
        public string UnApprovedTaskCode;
        public string WaitingTaskCode;
    }

    public class TasksCoreDependencies
    {
        public IDatabase Database;
    }
}