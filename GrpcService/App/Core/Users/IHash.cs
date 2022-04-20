namespace GrpcService1.App.Core.Users;

public interface IHash
{
    public string Hash(string expression);
    public bool VerifyHash(string hash, string original);
}