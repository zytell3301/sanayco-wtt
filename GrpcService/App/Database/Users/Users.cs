using GrpcService1.App.Core.Users;
using GrpcService1.App.Database.Model;
using Token = GrpcService1.Domain.Entities.Token;
using User = GrpcService1.Domain.Entities.User;

namespace GrpcService1.App.Database.Users;

public class Users : IDatabase
{
    private wttContext Connection;

    public Users(wttContext connection)
    {
        Connection = connection;
    }

    public User GetUser(User user)
    {
        throw new NotImplementedException();
    }

    public void RecordToken(Token token)
    {
        throw new NotImplementedException();
    }

    public void RecordUser(User user)
    {
        throw new NotImplementedException();
    }

    public User GetUserByUsername(User user)
    {
        throw new NotImplementedException();
    }
}