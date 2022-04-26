#region

using GrpcService1.Domain.Entities;

#endregion

namespace GrpcService1.App.Core.Users;

public interface IDatabase
{
    public User GetUser(User user);
    public void RecordToken(Token token);
    public int RecordUser(User user);
    public User GetUserByUsername(User user);
    public IRecordPermissionsBatch NewPermissionBatch();
    public void DeleteUserByUsername(User user);
    public List<Permission> GetUserPermissions(User user);
}

public interface IRecordPermissionsBatch
{
    public void AddPermissionToBatch(Permission permission);
    public void SaveChanges();
}