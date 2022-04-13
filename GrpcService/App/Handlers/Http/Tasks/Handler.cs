using GrpcService1.App.Handlers.Http.tasks.Validations;
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
    public string RecordTask([FromForm] string description, [FromForm] int project_id, [FromForm] int end_time,
        [FromForm] string title, [FromForm] string work_location, [FromForm] int user_id)
    {
        Domain.Entities.Task task = new Domain.Entities.Task()
        {
            Description = description,
            Title = title,
            EndTime = DateTime.UnixEpoch.AddSeconds(end_time),
            ProjectId = project_id,
            WorkLocation = work_location,
            UserId = user_id,
        };

        switch (ModelState.IsValid)
        {
            case false:
                // @TODO A proper error must be returned to the client
                return "Data validation failed";
        }

        try
        {
            Core.RecordTask(task);
        }
        catch (Exception e)
        {
            // @TODO A proper error must be returned to the client
            return "internal error";
        }

        return "operation successful";
    }

    [Route("delete-task")]
    public string DeleteTask([FromForm] int task_id)
    {
        DeleteTaskValidation validation = new DeleteTaskValidation()
        {
            Id = task_id,
        };
        switch (ModelState.IsValid)
        {
            case false:
                // @TODO A proper error must be returned to the client
                break;
        }

        try
        {
            Core.DeleteTask(new Domain.Entities.Task()
            {
                Id = task_id,
            });
        }
        catch (Exception e)
        {
            // @TODO A proper error must be returned to the client
            return "internal error";
        }

        return "operation successful";
    }

    [Route("edit-task")]
    public string EditTask([FromForm] int task_id, [FromForm] string description, [FromForm] int end_time,
        [FromForm] string title, [FromForm] string work_location, [FromForm] int user_id)
    {
        EditTaskValidation validation = new EditTaskValidation()
        {
            Description = description,
            EndTime = end_time,
            Title = title,
            WorkLocation = work_location,
            UserId = user_id,
            Id = task_id,
        };
        switch (ModelState.IsValid)
        {
            case false:
                // @TODO A proper response must be returned to the user because of invalid data
                return "Form validation failed";
        }

        try
        {
            Core.EditTask(new Domain.Entities.Task()
            {
                Id = task_id,
                Description = validation.Description,
                WorkLocation = validation.WorkLocation,
                UserId = validation.UserId,
                Title = validation.Title,
                EndTime = DateTime.UnixEpoch.AddSeconds(validation.EndTime),
            });
        }
        catch (Exception e)
        {
            return "an internal error occurred";
        }

        return "operation successful";
    }
}