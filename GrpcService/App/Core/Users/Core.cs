using GrpcService1.Domain.Entities;
using GrpcService1.Domain.Errors;

namespace GrpcService1.App.Core.Users;

public class Core
{
    private IDatabase Database;
    private IHash Hash;
    private ITokenGenerator TokenGenerator;

    private readonly InvalidCredentials InvalidCredentials;
    private readonly InternalError InternalError;

    private readonly int ExpirationWindow;

    public class UsersCoreConfigs
    {
        public string InvalidCredentialsMessage;
        public string InternalErrorMessage;

        public int ExpirationWindow;
    }

    public class UsersCoreDependencies
    {
        public IDatabase Database;
        public IHash Hash;
        public ITokenGenerator TokenGenerator;
    }

    public Core(UsersCoreConfigs configs, UsersCoreDependencies dependencies)
    {
        Database = dependencies.Database;
        Hash = dependencies.Hash;
        TokenGenerator = dependencies.TokenGenerator;

        InvalidCredentials = new InvalidCredentials(configs.InvalidCredentialsMessage);
        InternalError = new InternalError(configs.InternalErrorMessage);

        ExpirationWindow = configs.ExpirationWindow;
    }

    public Domain.Entities.Token Login(User user, string password)
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

            var token = new Token()
            {
                Token1 = TokenGenerator.GenerateToken(),
                UserId = user.Id,
                ExpirationDate = DateTime.Now.AddSeconds(ExpirationWindow),
            };

            Database.RecordToken(token);

            return token;
        }
        catch (Exception e)
        {
            throw InternalError;
        }
    }
}