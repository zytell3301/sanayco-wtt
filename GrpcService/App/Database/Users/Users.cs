#region

using ErrorReporter;
using GrpcService1.App.Core.Users;
using GrpcService1.App.Database.Model;
using GrpcService1.Domain.Errors;
using Permission = GrpcService1.Domain.Entities.Permission;
using Token = GrpcService1.Domain.Entities.Token;
using User = GrpcService1.Domain.Entities.User;

#endregion

namespace GrpcService1.App.Database.Users;

public class Users : IDatabase
{
    private readonly wttContext Connection;
    private readonly IErrorReporter ErrorReporter;
    private readonly InternalError InternalError;

    public Users(UsersDatabaseDependencies dependencies)
    {
        Connection = dependencies.Connection;
        ErrorReporter = dependencies.ErrorReporter;

        InternalError = dependencies.InternalError;
    }

    public User GetUser(User user)
    {
        try
        {
            return ConvertModelToUser(Connection.Users.First(u => u.Id == user.Id));
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
    }

    public void RecordToken(Token token)
    {
        try
        {
            Connection.Tokens.Add(new Model.Token
            {
                Token1 = token.Token1,
                UserId = token.UserId,
                ExpirationDate =
                    DateTime.Now
                        .AddSeconds(
                            1000000) // @TODO For test purposes expiration time is set almost permanent. Change it in production
            });
            Connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
    }

    public int RecordUser(User user)
    {
        try
        {
            var model = new Model.User
            {
                Lastname = user.LastName,
                Name = user.Name,
                Password = user.Password,
                Username = user.Username,
                CompanyLevel = user.CompanyLevel,
                SkillLevel = user.SkillLevel
            };
            Connection.Add(model);
            Connection.SaveChanges();
            return model.Id;
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
    }

    public User GetUserByUsername(User user)
    {
        try
        {
            return ConvertModelToUser(Connection.Users.First(u => u.Username == user.Username));
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
    }

    public IRecordPermissionsBatch NewPermissionBatch()
    {
        return new RecordPermissionsBatch(Connection);
    }

    public void DeleteUserByUsername(User user)
    {
        try
        {
            var model = Connection.Users.First(u => u.Username == user.Username);
            Connection.Users.Remove(model);
            Connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
    }

    public List<Permission> GetUserPermissions(User user)
    {
        var permissions = new List<Permission>();
        try
        {
            foreach (var permission in Connection.Permissions.Where(u => u.UserId == user.Id).ToList())
                permissions.Add(ConverModelToPermission(permission));

            return permissions;
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public Core.Users.UpdateUserBatch NewUpdateUserBatch(User user)
    {
        try
        {
            return new UpdateUserBatch(Connection, user, ErrorReporter, InternalError);
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
    }

    private User ConvertModelToUser(Model.User model)
    {
        var user = new User
        {
            Id = model.Id,
            Name = model.Name,
            Password = model.Password,
            CompanyLevel = model.CompanyLevel,
            LastName = model.Lastname,
            SkillLevel = model.SkillLevel,
            Username = model.Username
        };

        switch (model.CreatedAt.HasValue)
        {
            case true:
                user.CreatedAt = model.CreatedAt.Value;
                break;
        }

        return user;
    }

    private Permission ConverModelToPermission(Model.Permission model)
    {
        return new Permission
        {
            Id = model.Id,
            Title = model.Title,
            UserId = model.UserId.Value,
            GrantedBy = model.GrantedBy.Value,
            CreatedAt = model.CreatedAt.Value
        };
    }

    public class UsersDatabaseDependencies
    {
        public wttContext Connection;
        public IErrorReporter ErrorReporter;

        public InternalError InternalError;
    }
}