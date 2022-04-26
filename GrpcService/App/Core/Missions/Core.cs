using GrpcService1.Domain.Errors;

namespace GrpcService1.App.Core.Missions;

public class Core
{
    private IDatabase Database;
    private readonly InternalError InternalError;

    public class MissionsCoreDependencies
    {
        public IDatabase Database;
    }

    public class MissionsCoreConfigs
    {
        public string InternalErrorMessage;
    }

    public Core(MissionsCoreDependencies dependencies, MissionsCoreConfigs configs)
    {
        Database = dependencies.Database;
        InternalError = new InternalError(configs.InternalErrorMessage);
    }
}