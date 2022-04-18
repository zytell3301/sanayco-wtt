using System.Text.Json;
using GrpcService1.App.Handlers.Http.tasks.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace GrpcService1.App.Handlers.Http.tasks;

[Route("/tasks")]
public class Handler : BaseHandler
{
    private Core.Tasks.Core Core;

    public Handler(Core.Tasks.Core core)
    {
        Core = core;
    }

    [HttpPost("submit-task")]
    public string RecordTask()
    {
        var body = DecodePayloadJson<RecordTaskValidaion>();

        switch (ModelState.IsValid)
        {
            case false:
                // @TODO A proper error must be returned to the client
                return "Data validation failed";
        }

        try
        {
            Core.RecordTask(new Domain.Entities.Task()
            {
                Description = body.description,
                Title = body.title,
                EndTime = DateTime.UnixEpoch.AddSeconds(body.end_time),
                ProjectId = body.project_id,
                WorkLocation = body.work_location,
                UserId = body.user_id,
            });
        }
        catch (Exception e)
        {
            // @TODO A proper error must be returned to the client
            return "internal error";
        }

        return "operation successful";
    }

    [HttpPost("delete-task")]
    public string DeleteTask()
    {
        var body = DecodePayloadJson<DeleteTaskValidation>();

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
                Id = body.task_id,
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

    class GetTaskResponse
    {
        public Domain.Entities.Task Task { get; set; }

        // This is the status code that indicates the response status.
        // Status code 0 is always for successful operation.
        public int Code { get; set; }
    }

    [HttpGet("get-task/{id}")]
    public string GetTask(int id)
    {
        try
        {
            var task = Core.GetTask(new Domain.Entities.Task()
            {
                Id = id,
            });
            return JsonSerializer.Serialize(new GetTaskResponse()
            {
                Code = 0,
                Task = task,
            });
        }
        catch (Exception e)
        {
            return JsonSerializer.Serialize(new GetTaskResponse()
            {
                Code = 1,
            });
        }
    }
}