using GrpcService1.Domain.Entities;

namespace GrpcService1.App.Core.Users;

public interface IDatabase
{
    public User GetUser(User user);
    public void RecordToken(Domain.Entities.Token token);
    public void RecordUser(Domain.Entities.User user);
}