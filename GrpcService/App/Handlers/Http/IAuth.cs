namespace GrpcService1.App.Handlers.Http;

public interface IAuth
{
    public string ExtractToken(string token);
}