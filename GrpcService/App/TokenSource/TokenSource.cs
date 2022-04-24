using GrpcService1.App.Database.Model;
using GrpcService1.App.Handlers.Http;

namespace GrpcService1.App.TokenSource;

public class TokenSource : ITokenSource
{
    private wttContext Connection;

    public TokenSource(Database.Model.wttContext connection)
    {
        Connection = connection;
    }

    public int GetTokenUserId(string token)
    {
        return Connection.Tokens.Where(token1 => token1.Token1 == token).First().UserId.Value;
    }
}