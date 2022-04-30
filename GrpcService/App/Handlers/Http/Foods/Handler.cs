namespace GrpcService1.App.Handlers.Http.Foods;

public class Handler : BaseHandler
{
    private Core.Foods.Core Core;

    public Handler(Core.Foods.Core core, BaseHandlerDependencies baseHandlerDependencies) : base(
        baseHandlerDependencies)
    {
        Core = core;
    }
}