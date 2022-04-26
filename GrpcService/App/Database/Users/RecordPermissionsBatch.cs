#region

using GrpcService1.App.Core.Users;
using GrpcService1.App.Database.Model;
using GrpcService1.Domain.Errors;
using Permission = GrpcService1.Domain.Entities.Permission;

#endregion

namespace GrpcService1.App.Database.Users;

public class RecordPermissionsBatch : IRecordPermissionsBatch
{
    private readonly wttContext Connection;

    public RecordPermissionsBatch(wttContext connection)
    {
        Connection = connection;
    }

    public void AddPermissionToBatch(Permission permission)
    {
        try
        {
            Connection.Permissions.Add(new Model.Permission
            {
                Title = permission.Title,
                GrantedBy = permission.GrantedBy,
                UserId = permission.UserId
            });
        }
        catch (Exception)
        {
            throw new InternalError("internal error occurred");
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
            throw new InternalError("internal error occurred");
        }
    }
}