#region

using System.Text.Json;
using GrpcService1.App.Handlers.Http.tasks.Validations;
using GrpcService1.Domain.Errors;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace GrpcService1.App.Handlers.Http.tasks;

[Route("/tasks")]
public class Handler : BaseHandler
{
    private readonly Core.Tasks.Core Core;

    public Handler(Core.Tasks.Core core, ITokenSource tokenSource, AuthenticationFailed authenticationFailed) : base(tokenSource,
        authenticationFailed)
    {
        Core = core;
    }

    [HttpPost("submit-task")]
    public string RecordTask()
    {
        RecordTaskValidaion body;
        try
        {
            body = DecodePayloadJson<RecordTaskValidaion>();
        }
        catch (Exception e)
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store  current request data for ip blacklist analysis
            return InvalidRequestResponse;
        }

        switch (ModelState.IsValid)
        {
            case false:
                // @TODO A proper error must be returned to the client
                return "Data validation failed";
        }

        try
        {
            Core.RecordTask(new Domain.Entities.Task
            {
                Description = body.description,
                Title = body.title,
                EndTime = DateTime.UnixEpoch.AddSeconds(body.end_time),
                ProjectId = body.project_id,
                WorkLocation = body.work_location,
                UserId = body.user_id
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
        DeleteTaskValidation body;
        try
        {
            body = DecodePayloadJson<DeleteTaskValidation>();
        }
        catch (Exception e)
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store  current request data for ip blacklist analysis
            return InvalidRequestResponse;
        }

        switch (ModelState.IsValid)
        {
            case false:
                // @TODO A proper error must be returned to the client
                break;
        }

        try
        {
            Core.DeleteTask(new Domain.Entities.Task
            {
                Id = body.task_id
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
    public string EditTask()
    {
        EditTaskValidation body;
        try
        {
            body = DecodePayloadJson<EditTaskValidation>();
        }
        catch (Exception e)
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store  current request data for ip blacklist analysis
            return InvalidRequestResponse;
        }

        switch (ModelState.IsValid)
        {
            case false:
                // @TODO A proper response must be returned to the user because of invalid data
                return "Form validation failed";
        }

        try
        {
            Core.EditTask(new Domain.Entities.Task
            {
                Id = body.task_id,
                Description = body.description,
                WorkLocation = body.work_location,
                Title = body.title,
                EndTime = DateTime.UnixEpoch.AddSeconds(body.end_time)
            });
        }
        catch (Exception e)
        {
            return "an internal error occurred";
        }

        return "operation successful";
    }

    [HttpGet("get-task/{id}")]
    public string GetTask(int id)
    {
        try
        {
            var task = Core.GetTask(new Domain.Entities.Task
            {
                Id = id
            });
            return JsonSerializer.Serialize(new GetTaskResponse
            {
                Code = 0,
                Task = task
            });
        }
        catch (Exception e)
        {
            return JsonSerializer.Serialize(new GetTaskResponse
            {
                Code = 1
            });
        }
    }

    [HttpPost("approve-task")]
    public string ApproveTask()
    {
        UpdateTaskStatusValidation body;
        try
        {
            body = DecodePayloadJson<UpdateTaskStatusValidation>();
        }
        catch (Exception e)
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store  current request data for ip blacklist analysis
            return InvalidRequestResponse;
        }

        switch (ModelState.IsValid)
        {
            case false:
                // @TODO A proper error must be returned to user because of invalid data.
                return "data validation failed";
        }

        try
        {
            Core.ApproveTask(new Domain.Entities.Task
            {
                Id = body.task_id
            });
        }
        catch (Exception e)
        {
            // @TODO A proper error must be returned to user because of internal error
            return "operation failed";
        }

        return "operation successful";
    }

    [HttpPost("reject-task")]
    public string RejectTask()
    {
        UpdateTaskStatusValidation body;
        try
        {
            body = DecodePayloadJson<UpdateTaskStatusValidation>();
        }
        catch (Exception e)
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store  current request data for ip blacklist analysis
            return InvalidRequestResponse;
        }

        switch (ModelState.IsValid)
        {
            case false:
                // @TODO A proper error must returned to user for invalid data
                return "data validation failed";
        }

        try
        {
            Core.RejectTask(new Domain.Entities.Task
            {
                Id = body.task_id
            });
        }
        catch (Exception e)
        {
            // @TODO A proper error must be returned to user for internal error
            return "operation failed";
        }

        return "operation successful";
    }

    [HttpPost("set-task-waiting")]
    public string SetTaskWaiting()
    {
        UpdateTaskStatusValidation body;
        try
        {
            body = DecodePayloadJson<UpdateTaskStatusValidation>();
        }
        catch (Exception e)
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store  current request data for ip blacklist analysis
            return InvalidRequestResponse;
        }

        switch (ModelState.IsValid)
        {
            case false:
                // @TODO A proper error must be returned to user for invalid data
                return "data validation failed";
        }

        try
        {
            Core.SetTaskWaiting(new Domain.Entities.Task
            {
                Id = body.task_id
            });
        }
        catch (Exception e)
        {
            //@TODO A proper error must be returned to user for internal failure
            return "operation failed";
        }

        return "operation successful";
    }

    private class GetTaskResponse
    {
        public Domain.Entities.Task Task { get; set; }

        // This is the status code that indicates the response status.
        // Status code 0 is always for successful operation.
        public int Code { get; set; }
    }

    [HttpGet("/test")]
    public string Test()
    {
        Console.WriteLine(Authenticate());
        return ":D";
    }
}