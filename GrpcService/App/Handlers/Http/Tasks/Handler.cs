﻿#region

using System.Text.Json;
using GrpcService1.App.Handlers.Http.tasks.Validations;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace GrpcService1.App.Handlers.Http.tasks;

[Route("/tasks")]
public class Handler : BaseHandler
{
    private readonly Core.Tasks.Core Core;

    public Handler(Core.Tasks.Core core, BaseHandlerDependencies baseHandlerDependencies) : base(
        baseHandlerDependencies)
    {
        Core = core;
    }

    [HttpPost("submit-task")]
    public string RecordTask()
    {
        try
        {
            Authorize("create-task");
        }
        catch (Exception)
        {
            return ResponseToJson(AuthorizationFailedResponse());
        }

        RecordTaskValidaion body;
        try
        {
            body = DecodePayloadJson<RecordTaskValidaion>();
        }
        catch (Exception)
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store  current request data for ip blacklist analysis
            return InvalidRequestResponse;
        }

        switch (ModelState.IsValid)
        {
            case false:
                return ResponseToJson(DataValidationFailedResponse());
        }

        try
        {
            Core.RecordTask(new Domain.Entities.Task
            {
                Description = body.description,
                Title = body.title,
                ProjectId = body.project_id,
                WorkLocation = body.work_location,
                UserId = GetUserId(),
                Points = body.points,
                StartTime = DateTime.UnixEpoch.AddSeconds(body.start_time),
            });
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }

    [HttpPost("delete-task")]
    public string DeleteTask()
    {
        DeleteTaskValidation body;
        try
        {
            body = DecodePayloadJson<DeleteTaskValidation>();
        }
        catch (Exception)
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store  current request data for ip blacklist analysis
            return InvalidRequestResponse;
        }

        try
        {
            switch (Core.CheckTaskOwnership(body.task_id, GetUserId()))
            {
                case false:
                    try
                    {
                        Authorize("delete-task");
                    }
                    catch (Exception)
                    {
                        return ResponseToJson(AuthorizationFailedResponse());
                    }

                    break;
            }
        }
        catch (Exception)
        {
            return ResponseToJson(AuthorizationFailedResponse());
        }

        switch (ModelState.IsValid)
        {
            case false:
                return ResponseToJson(DataValidationFailedResponse());
        }

        try
        {
            Core.DeleteTask(new Domain.Entities.Task
            {
                Id = body.task_id
            });
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }

    [Route("edit-task")]
    public string EditTask()
    {
        EditTaskValidation body;
        try
        {
            body = DecodePayloadJson<EditTaskValidation>();
        }
        catch (Exception)
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store  current request data for ip blacklist analysis
            return InvalidRequestResponse;
        }

        try
        {
            switch (Core.CheckTaskOwnership(body.task_id, GetUserId()))
            {
                case false:
                    Authorize("edit-task");
                    break;
            }
        }
        catch (Exception)
        {
            return ResponseToJson(AuthorizationFailedResponse());
        }

        switch (ModelState.IsValid)
        {
            case false:
                return ResponseToJson(DataValidationFailedResponse());
        }

        try
        {
            Core.EditTask(new Domain.Entities.Task
            {
                Id = body.task_id,
                Description = body.description,
                WorkLocation = body.work_location,
                Title = body.title,
            });
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }

    [HttpGet("get-task/{id}")]
    public string GetTask(int id)
    {
        try
        {
            Authenticate();
        }
        catch (Exception)
        {
            return ResponseToJson(AuthenticationFailedResponse());
        }

        try
        {
            var task = Core.GetTask(new Domain.Entities.Task
            {
                Id = id
            });
            return JsonSerializer.Serialize(new GetTaskResponse
            {
                status_code = 0,
                Task = task
            });
        }
        catch (Exception)
        {
            return JsonSerializer.Serialize(new GetTaskResponse
            {
                status_code = 1
            });
        }
    }

    [HttpPost("approve-task")]
    public string ApproveTask()
    {
        try
        {
            Authorize("change-task-status");
        }
        catch (Exception)
        {
            return ResponseToJson(AuthorizationFailedResponse());
        }

        UpdateTaskStatusValidation body;
        try
        {
            body = DecodePayloadJson<UpdateTaskStatusValidation>();
        }
        catch (Exception)
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store  current request data for ip blacklist analysis
            return InvalidRequestResponse;
        }

        switch (ModelState.IsValid)
        {
            case false:
                return ResponseToJson(DataValidationFailedResponse());
        }

        try
        {
            Core.ApproveTask(new Domain.Entities.Task
            {
                Id = body.task_id
            });
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }

    [HttpPost("reject-task")]
    public string RejectTask()
    {
        try
        {
            Authorize("change-task-status");
        }
        catch (Exception)
        {
            return ResponseToJson(AuthorizationFailedResponse());
        }

        UpdateTaskStatusValidation body;
        try
        {
            body = DecodePayloadJson<UpdateTaskStatusValidation>();
        }
        catch (Exception)
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store  current request data for ip blacklist analysis
            return InvalidRequestResponse;
        }

        switch (ModelState.IsValid)
        {
            case false:
                return ResponseToJson(DataValidationFailedResponse());
        }

        try
        {
            Core.RejectTask(new Domain.Entities.Task
            {
                Id = body.task_id
            });
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }

    [HttpPost("set-task-waiting")]
    public string SetTaskWaiting()
    {
        try
        {
            Authorize("change-task-status");
        }
        catch (Exception)
        {
            return ResponseToJson(AuthorizationFailedResponse());
        }

        UpdateTaskStatusValidation body;
        try
        {
            body = DecodePayloadJson<UpdateTaskStatusValidation>();
        }
        catch (Exception)
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store  current request data for ip blacklist analysis
            return InvalidRequestResponse;
        }

        switch (ModelState.IsValid)
        {
            case false:
                return ResponseToJson(DataValidationFailedResponse());
        }

        try
        {
            Core.SetTaskWaiting(new Domain.Entities.Task
            {
                Id = body.task_id
            });
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }

    private class GetTaskResponse : Response
    {
        public Domain.Entities.Task Task { get; set; }
    }
}