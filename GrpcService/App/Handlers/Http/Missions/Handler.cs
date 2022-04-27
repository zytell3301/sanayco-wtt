using Microsoft.AspNetCore.Mvc;

namespace GrpcService1.App.Handlers.Http.Missions;

[Route("/missions")]
public class Handler : BaseHandler
{
    private Core.Missions.Core Core;

    public Handler(Core.Missions.Core core, BaseHandlerDependencies baseHandlerDependencies) : base(
        baseHandlerDependencies)
    {
        Core = core;
    }
}