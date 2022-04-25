namespace GrpcService1.App.Handlers.Http.Users;

public class Handler : BaseHandler
{
    private Core.Users.Core Core;

    public Handler(Core.Users.Core core, BaseHandlerDependencies baseHandlerDependencies) : base(
        baseHandlerDependencies)
    {
        Core = core;
    }
}