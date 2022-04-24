using GrpcService1.App.Handlers.Http;

namespace GrpcService1.App.PermissionsSource;

public class PermissionsSource : IPermissionsSource
{
    private Database.Model.wttContext Connection;

    public PermissionsSource(Database.Model.wttContext connection)
    {
        Connection = connection;
    }

    public bool CheckPermission(int userId, string permission)
    {
        try
        {
            Connection.Permissions.Where(p => p.UserId == userId).First(p => p.Title == permission);
            return
                true; // If the code reaches here it means that the previous line didn't throw a record not found exception
        }
        catch (Exception e)
        {
            return false;
        }
    }
}