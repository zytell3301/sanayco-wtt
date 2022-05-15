#region

using GrpcService1.App.Database.Model;
using GrpcService1.App.Handlers.Http;
using GrpcService1.Domain.Errors;
using Permission = GrpcService1.Domain.Entities.Permission;

#endregion

namespace GrpcService1.App.PermissionsSource;

public class PermissionsSource : IPermissionsSource
{
    private readonly wttContext Connection;

    public PermissionsSource(wttContext connection)
    {
        Connection = connection;
    }

    public bool CheckPermission(int userId, string permission)
    {
        try
        {
            GetUserPermissionQuery(userId).First(p => p.Title == permission);
            return
                true; // If the code reaches here it means that the previous line didn't throw a record not found exception
        }
        catch (Exception)
        {
            return false;
        }
    }

    private IQueryable<Database.Model.Permission> GetUserPermissionQuery(int userId)
    {
        return Connection.Permissions.Where(p => p.UserId == userId);
    }

    public List<Permission> GetUserPermissions(int userId)
    {
        var permissions = new List<Permission>();
        try
        {
            foreach (var permission in GetUserPermissionQuery(userId).ToList())
            {
                permissions.Add(ConvertPermissionModel(permission));
            }

            return permissions;
        }
        catch (Exception)
        {
            throw new InternalError("");
        }
    }

    private Permission ConvertPermissionModel(Database.Model.Permission model)
    {
        return new Permission()
        {
            Id = model.Id,
            Title = model.Title,
            CreatedAt = model.CreatedAt.Value,
            GrantedBy = model.GrantedBy.Value,
            UserId = model.UserId.Value,
        };
    }
}