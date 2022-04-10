using GrpcService1.Domain.Errors;

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
}