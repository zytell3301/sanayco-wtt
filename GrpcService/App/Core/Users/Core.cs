#region

using GrpcService1.App.Handlers.Http;
using GrpcService1.Domain.Entities;
using GrpcService1.Domain.Errors;

#endregion

namespace GrpcService1.App.Core.Users;

public class Core
{
    private readonly IAuth Auth;
    private readonly IDatabase Database;
    private readonly int ExpirationWindow;
    private readonly IHash Hash;
    private readonly InternalError InternalError;

    private readonly InvalidCredentials InvalidCredentials;
    private readonly IPermissionsSource PermissionsSource;
    private readonly IProfilePicturesStorage ProfilePicturesStorage;
    private readonly ITokenGenerator TokenGenerator;

    public Core(UsersCoreConfigs configs, UsersCoreDependencies dependencies)
    {
        Database = dependencies.Database;
        Hash = dependencies.Hash;
        TokenGenerator = dependencies.TokenGenerator;
        ProfilePicturesStorage = dependencies.ProfilePicturesStorage;
        PermissionsSource = dependencies.PermissionsSource;
        Auth = dependencies.Auth;

        InvalidCredentials = new InvalidCredentials(configs.InvalidCredentialsMessage);
        InternalError = new InternalError(configs.InternalErrorMessage);

        ExpirationWindow = configs.ExpirationWindow;
    }

    public string Login(User user, string password)
    {
        try
        {
            user = Database.GetUserByUsername(user);
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

            var permissions = PermissionsSource.GetUserPermissions(user.Id);

            var authToken = Auth.GenerateAuthToken(token, permissions);

            Database.RecordToken(token);

            return authToken;
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public void Register(User user, List<Permission> permissions, IFormFile profilePicture)
    {
        try
        {
            // var fileName = ProfilePicturesStorage.StoreProfilePicture(profilePicture);
            var UserId = Database.RecordUser(new User
            {
                Name = user.Name,
                Password = Hash.Hash(user.Password),
                CompanyLevel = user.CompanyLevel,
                LastName = user.LastName,
                SkillLevel = user.SkillLevel,
                Username = user.Username
            });
            var permissionsBatch = Database.NewPermissionBatch();
            foreach (var permission in permissions)
            {
                permission.UserId = UserId;
                permissionsBatch.AddPermissionToBatch(permission);
            }

            permissionsBatch.SaveChanges();
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public void DeleteUser(User user)
    {
        try
        {
            Database.DeleteUserByUsername(user);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public User GetUserByUsername(User user)
    {
        try
        {
            return Database.GetUserByUsername(user);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public List<Permission> GetUserPermissions(User user)
    {
        try
        {
            return Database.GetUserPermissions(user);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public void UpdateUser(User user, List<Permission> permissions)
    {
        try
        {
            var batch = Database.NewUpdateUserBatch(user);
            batch.RevokeAllPermissions(); // Since we are receiving a new list of permissions, it is required to revoke all permissions first and then insert new ones.
            batch.SaveChanges();

            batch.UpdateUser(user);
            batch.AddPermission(permissions);
            batch.SaveChanges();
        }
        catch (Exception)
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
        public IAuth Auth;
        public IDatabase Database;
        public IHash Hash;
        public IPermissionsSource PermissionsSource;
        public IProfilePicturesStorage ProfilePicturesStorage;
        public ITokenGenerator TokenGenerator;
    }
}