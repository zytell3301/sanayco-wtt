using Confluent.Kafka;
using ErrorReporter;
using GrpcService1.App.Database.Model;
using GrpcService1.Domain.Errors;
using Permission = GrpcService1.Domain.Entities.Permission;
using User = GrpcService1.Domain.Entities.User;

namespace GrpcService1.App.Database.Users;

public class UpdateUserBatch : Core.Users.UpdateUserBatch
{
    private wttContext Connection;
    private Model.User User;
    private readonly InternalError InternalError;
    private IErrorReporter ErrorReporter;

    public UpdateUserBatch(wttContext connection, User user, IErrorReporter errorReporter, InternalError internalError)
    {
        Connection = connection;
        User = Connection.Users.First(u => u.Id == user.Id);
        InternalError = internalError;
        ErrorReporter = errorReporter;
    }

    public void UpdateUser(User user)
    {
        try
        {
            User.Lastname = user.LastName;
            User.Name = user.Name;
            User.SkillLevel = user.SkillLevel;
            User.CompanyLevel = user.CompanyLevel;
            User.Username = user.Username;
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
    }

    public void AddPermission(List<Permission> permissions)
    {
        try
        {
            foreach (var permission in permissions)
            {
                Console.WriteLine(permission.UserId);
                Connection.Permissions.Add(new Model.Permission()
                {
                    Title = permission.Title,
                    GrantedBy = permission.GrantedBy,
                    UserId = permission.UserId,
                });
            }
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
    }

    public void RevokeAllPermissions()
    {
        try
        {
            Connection.Permissions.RemoveRange(Connection.Permissions.Where(p => p.UserId == User.Id).AsEnumerable());
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
    }

    public void SaveChanges()
    {
        try
        {
            Connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
    }
}