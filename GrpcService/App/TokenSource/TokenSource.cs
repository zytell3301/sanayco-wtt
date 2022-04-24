#region

using GrpcService1.App.Database.Model;
using GrpcService1.App.Handlers.Http;

#endregion

namespace GrpcService1.App.TokenSource;

public class TokenSource : ITokenSource
{
    private readonly wttContext Connection;

    public TokenSource(wttContext connection)
    {
        Connection = connection;
    }

    public int GetTokenUserId(string token)
    {
        return Connection.Tokens.Where(token1 => token1.Token1 == token).First().UserId.Value;
    }
}