#region

using GrpcService1.Domain.Entities;

#endregion

namespace GrpcService1.App.Core.Users;

public interface IAuth
{
    public string GenerateAuthToken(Token token, List<Permission> permissions);
}