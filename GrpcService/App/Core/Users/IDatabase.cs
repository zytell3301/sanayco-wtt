#region

using GrpcService1.Domain.Entities;

#endregion

namespace GrpcService1.App.Core.Users;

public interface IDatabase
{
    public User GetUser(User user);
    public void RecordToken(Token token);
    public void RecordUser(User user);
}