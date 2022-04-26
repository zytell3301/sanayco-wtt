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

    // This method only uses user parameter to get the user model from database by primary keys. No changes applies via this method.
    // For applying any changes you must call UpdateUser method explicitly.
    public UpdateUserBatch NewUpdateUserBatch(User user);
}

public interface UpdateUserBatch
{
    public void UpdateUser(User user);
    public void AddPermission(List<Permission> permissions);
    public void RevokeAllPermissions();
    public void SaveChanges();
}

public interface IRecordPermissionsBatch
{
    public void AddPermissionToBatch(Permission permission);
    public void SaveChanges();
}