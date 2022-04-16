using Microsoft.AspNetCore.Mvc;

namespace GrpcService1.App.Handlers.Http.Presentation;

public class Handler : ControllerBase
{
    private Core.Presentation.Core Core;

    public Handler(Core.Presentation.Core core)
    {
        Core = core;
    }
}