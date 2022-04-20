﻿using GrpcService1.Domain.Entities;
using GrpcService1.Domain.Errors;

namespace GrpcService1.App.Core.Users;

public class Core
{
    private IDatabase Database;
    private IHash Hash;

    private readonly InvalidCredentials InvalidCredentials;
    private readonly InternalError InternalError;

    public class UsersCoreConfigs
    {
        public string InvalidCredentialsMessage;
        public string InternalErrorMessage;
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

        InvalidCredentials = new InvalidCredentials(configs.InvalidCredentialsMessage);
        InternalError = new InternalError(configs.InternalErrorMessage);
    }
}