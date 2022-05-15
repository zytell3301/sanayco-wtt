using GrpcService1.Domain.Entities;

namespace GrpcService1.App.Handlers.Http;

public interface IPermissionsSource
{
    public bool CheckPermission(int userId, string permission);
    public List<Permission> GetUserPermissions(int userId);
}