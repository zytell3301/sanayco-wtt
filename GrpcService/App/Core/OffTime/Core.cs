using GrpcService1.Domain.Errors;

namespace GrpcService1.App.Core.OffTime;

public class Core
{
    private InternalError InternalError;
    private OperationSuccessful OperationSuccessful;
    private IDatabase Database;

    public class OffTimeCoreConfigs
    {
        public string OperationsuccessfulMessage;
        public string InternalErrorMessage;
    }

    public class OffTimeDependencies
    {
        public IDatabase Database;
    }

    public Core(OffTimeDependencies dependencies, OffTimeCoreConfigs configs)
    {
        InternalError = new InternalError(configs.InternalErrorMessage);
        OperationSuccessful = new OperationSuccessful(configs.OperationsuccessfulMessage);

        Database = dependencies.Database;
    }
}