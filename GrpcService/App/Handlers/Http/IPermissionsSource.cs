namespace GrpcService1.App.Handlers.Http;

public interface IPermissionsSource
{
    public bool CheckPermission(int userId, string permission);
}