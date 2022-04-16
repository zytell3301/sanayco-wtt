using Microsoft.AspNetCore.Mvc;

namespace GrpcService1.App.Handlers.Http.OffTime;

public class Handler : ControllerBase
{
    private Core.OffTime.Core Core;

    public Handler(Core.OffTime.Core core)
    {
        Core = core;
    }
}