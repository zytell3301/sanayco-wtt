using GrpcService1.Domain.Errors;
using Task = GrpcService1.Domain.Entities.Task;

namespace GrpcService1.App.Core.Tasks;

public class Core
{
    private OperationSuccessful OperationSuccessful;
    private InternalError InternalError;
    private IDatabase Database;

    public class TasksCoreConfigs
    {
        public string OperationSuccessfulmessage;
        public string InternalErrorMessage;
    }

    public class TasksCoreDependencies
    {
        public IDatabase Database;
    }

    public Core(TasksCoreDependencies dependencies, TasksCoreConfigs configs)
    {
        OperationSuccessful = new OperationSuccessful(configs.OperationSuccessfulmessage);
        InternalError = new InternalError(configs.InternalErrorMessage);
        Database = dependencies.Database;
    }

    /*
     * This method records the given task. 
     */
    public Status RecordTask(Task task)
    {
        try
        {
            Database.RecordTask(task);
        }
        catch (Exception e)
        {
            return InternalError;
        }

        return OperationSuccessful;
    }

    public Status DeleteTask(Task task)
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

    public Status EditTask(Task task)
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
}