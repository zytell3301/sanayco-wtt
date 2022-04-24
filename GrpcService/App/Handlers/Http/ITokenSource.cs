namespace GrpcService1.App.Handlers.Http;

public interface ITokenSource
{
    public int GetTokenUserId(string token);
}