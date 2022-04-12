using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace GrpcService1.App.Handlers.Http.tasks;

[Route("/tasks")]
public class Handler : ControllerBase
{
    private Core.Tasks.Core Core;

    public Handler(Core.Tasks.Core core)
    {
        Core = core;
    }

    [HttpPost("submit-task")]
    public string RecordTask(IFormCollection formCollection)
    {
        formCollection.TryGetValue("description", out var Description);
        formCollection.TryGetValue("project_id", out var DrojectId);
        formCollection.TryGetValue("end_time", out var EndTime);
        formCollection.TryGetValue("title", out var Ttitle);
        formCollection.TryGetValue("work_location", out var WorkLocation);
        switch (ModelState.IsValid)
        {
            case false:
                // A proper error must be returned to the client
                return "internal error";
                break;
        }

        try
        {
            Core.RecordTask(new Domain.Entities.Task()
            {
                Description = Description,
                Title = Ttitle,
                EndTime = DateTime.UnixEpoch.AddSeconds(Int32.Parse(EndTime)),
            });
        }
        catch (Exception e)
        {
            // @TODO A proper error must be returned to the client
            return "internal error";
        }

        return "operation successful";
    }
}