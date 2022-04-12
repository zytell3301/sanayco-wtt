using Microsoft.AspNetCore.Mvc;

namespace GrpcService1.App.Handlers.Http.tasks;

public class Handler : ControllerBase
{
    private Core.Tasks.Core Core;

    public Handler(Core.Tasks.Core core)
    {
        Core = core;
    }
}