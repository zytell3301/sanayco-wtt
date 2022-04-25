using ErrorReporter;
using GrpcService1.App.Core.Users;
using GrpcService1.App.Database.Model;
using GrpcService1.Domain.Errors;
using Token = GrpcService1.Domain.Entities.Token;
using User = GrpcService1.Domain.Entities.User;

namespace GrpcService1.App.Database.Users;

public class Users : IDatabase
{
    private wttContext Connection;
    private IErrorReporter ErrorReporter;
    private readonly InternalError InternalError;

    public class UsersDatabaseDependencies
    {
        public wttContext Connection;
        public IErrorReporter ErrorReporter;

        public InternalError InternalError;
    }

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
        throw new NotImplementedException();
    }

    public void RecordUser(User user)
    {
        throw new NotImplementedException();
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

    private Domain.Entities.User ConvertModelToUser(Model.User model)
    {
        var user = new Domain.Entities.User()
        {
            Id = model.Id,
            Name = model.Name,
            Password = model.Password,
            CompanyLevel = model.CompanyLevel,
            LastName = model.Lastname,
            SkillLevel = model.SkillLevel,
            Username = model.Username,
        };

        switch (model.CreatedAt.HasValue)
        {
            case true:
                user.CreatedAt = model.CreatedAt.Value;
                break;
        }

        return user;
    }
}