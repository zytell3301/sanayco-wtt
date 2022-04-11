using GrpcService1.Domain.Errors;
using Task = GrpcService1.Domain.Entities.Task;

namespace GrpcService1.App.Core.Tasks;

public class Core
{
    private OperationSuccessful OperationSuccessful;
    private InternalError InternalError;
    private IDatabase Database;

    public string ApprovedTaskCode;
    public string WaitingTaskCode;
    public string RejectedTaskCode;

    public class TasksCoreConfigs
    {
        public string OperationSuccessfulmessage;
        public string InternalErrorMessage;

        public string ApprovedTaskCode;
        public string WaitingTaskCode;
        public string UnApprovedTaskCode;
    }

    public class TasksCoreDependencies
    {
        public IDatabase Database;
    }

    public Core(TasksCoreDependencies dependencies, TasksCoreConfigs configs)
    {
        OperationSuccessful = new OperationSuccessful(configs.OperationSuccessfulmessage);
        InternalError = new InternalError(configs.InternalErrorMessage);
        ApprovedTaskCode = configs.ApprovedTaskCode;
        WaitingTaskCode = configs.WaitingTaskCode;
        RejectedTaskCode = configs.UnApprovedTaskCode;

        Database = dependencies.Database;
    }

    /*
     * This method records the given task. 
     */
    public Domain.Errors.Status RecordTask(Domain.Entities.Task task)
    {
        try
        {
            task.Status = WaitingTaskCode;
            Database.RecordTask(task);
        }
        catch (Exception e)
        {
            return InternalError;
        }

        return OperationSuccessful;
    }

    public Domain.Errors.Status DeleteTask(Domain.Entities.Task task)
    {
        try
        {
            Database.DeleteTask(task);
        }
        catch (Exception e)
        {
            return InternalError;
        }

        return OperationSuccessful;
    }

    public Domain.Errors.Status EditTask(Domain.Entities.Task task)
    {
        try
        {
            Database.EditTask(task);
        }
        catch (Exception e)
        {
            return InternalError;
        }

        return OperationSuccessful;
    }

    public Domain.Errors.Status ApproveTask(Domain.Entities.Task task)
    {
        try
        {
            Database.ChangeTaskStatus(task, ApprovedTaskCode);
        }
        catch (Exception e)
        {
            return InternalError;
        }

        return OperationSuccessful;
    }

    public Domain.Errors.Status RejectTask(Domain.Entities.Task task)
    {
        try
        {
            Database.ChangeTaskStatus(task, RejectedTaskCode);
        }
        catch (Exception e)
        {
            return InternalError;
        }

        return OperationSuccessful;
    }
}