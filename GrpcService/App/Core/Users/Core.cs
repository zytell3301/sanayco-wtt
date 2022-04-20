using GrpcService1.Domain.Entities;

namespace GrpcService1.App.Core.Users;

public class Core
{
    private IDatabase Database;
    private IHash Hash;

    public class UsersCoreConfigs
    {
    }

    public class UsersCoreDependencies
    {
        public IDatabase Database;
        public IHash Hash;
    }

    public Core(UsersCoreConfigs configs, UsersCoreDependencies dependencies)
    {
        Database = dependencies.Database;
        Hash = dependencies.Hash;
    }
}