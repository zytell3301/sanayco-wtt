﻿#region

using GrpcService1.Domain.Entities;
using GrpcService1.Domain.Errors;

#endregion

namespace GrpcService1.App.Core.Users;

public class Core
{
    private readonly int ExpirationWindow;
    private readonly InternalError InternalError;

    private readonly InvalidCredentials InvalidCredentials;
    private readonly IDatabase Database;
    private readonly IHash Hash;
    private readonly ITokenGenerator TokenGenerator;

    public Core(UsersCoreConfigs configs, UsersCoreDependencies dependencies)
    {
        Database = dependencies.Database;
        Hash = dependencies.Hash;
        TokenGenerator = dependencies.TokenGenerator;

        InvalidCredentials = new InvalidCredentials(configs.InvalidCredentialsMessage);
        InternalError = new InternalError(configs.InternalErrorMessage);

        ExpirationWindow = configs.ExpirationWindow;
    }

    public Token Login(User user, string password)
    {
        try
        {
            user = Database.GetUser(user);
            switch (Hash.VerifyHash(user.Password, password))
            {
                // Password hashes does not match. So the password is incorrect.
                // @TODO Number of wrong attempts must be recorded for every user to prevent from brute force attacks.
                case false:
                    throw InvalidCredentials;
            }

            var token = new Token
            {
                Token1 = TokenGenerator.GenerateToken(),
                UserId = user.Id,
                ExpirationDate = DateTime.Now.AddSeconds(ExpirationWindow)
            };

            Database.RecordToken(token);

            return token;
        }
        catch (Exception e)
        {
            throw InternalError;
        }
    }

    public void Register(User user)
    {
        try
        {
            Database.RecordUser(new User
            {
                Name = user.Name,
                Password = Hash.Hash(user.Password),
                CompanyLevel = user.CompanyLevel,
                LastName = user.LastName,
                SkillLevel = user.SkillLevel
            });
        }
        catch (Exception e)
        {
            throw InternalError;
        }
    }

    public class UsersCoreConfigs
    {
        public int ExpirationWindow;
        public string InternalErrorMessage;
        public string InvalidCredentialsMessage;
    }

    public class UsersCoreDependencies
    {
        public IDatabase Database;
        public IHash Hash;
        public ITokenGenerator TokenGenerator;
    }
}