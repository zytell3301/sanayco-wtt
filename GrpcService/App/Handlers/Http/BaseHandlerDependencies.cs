#region

using GrpcService1.Domain.Errors;

#endregion

namespace GrpcService1.App.Handlers.Http;

public class BaseHandlerDependencies
{
    public ITokenSource TokenSource { get; set; }
    public IPermissionsSource PermissionsSource { get; set; }
    public AuthenticationFailed AuthenticationFailed { get; set; }
    public AuthorizationFailed AuthorizationFailed { get; set; }
}