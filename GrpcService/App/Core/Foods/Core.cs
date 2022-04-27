using GrpcService1.Domain.Errors;

namespace GrpcService1.App.Core.Foods;

public class Core
{
    private readonly InternalError InternalError;

    private IDatabase Database;

    public class FoodsCoreDependencies
    {
        public IDatabase Database;
    }

    public class FoodsCoreConfigs
    {
        public string InternalErrorMessage;
    }

    public Core(FoodsCoreDependencies dependencies, FoodsCoreConfigs configs)
    {
        InternalError = new InternalError(configs.InternalErrorMessage);

        Database = dependencies.Database;
    }
}