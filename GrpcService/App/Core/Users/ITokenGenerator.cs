namespace GrpcService1.App.Core.Users;

public interface ITokenGenerator
{
    public string GenerateToken();
}