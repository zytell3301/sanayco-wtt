#region

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
                StartTime = DateTime.UnixEpoch.AddSeconds((double) (body.start_time / 1000))
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
                Points = body.points,
                StartTime = DateTime.UnixEpoch.AddSeconds((double) (body.start_time / 1000))
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

    [HttpGet("get-range/{fromDate}/{toDate}/{projectId}/{workLocation}")]
    public string GetTaskRange(int fromDate, int toDate, int projectId, string workLocation)
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
            return JsonSerializer.Serialize(new GetTaskRangeResponse
            {
                status_code = 0,
                tasks = Core.GetTaskRange(DateTime.UnixEpoch.AddSeconds(fromDate),
                    DateTime.UnixEpoch.AddSeconds(toDate), GetUserId(), projectId, workLocation)
            });
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }
    }

    [HttpGet("get-user-tasks/{fromDate}/{toDate}")]
    public string GetUserTasks(int fromDate, int toDate)
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
            return JsonSerializer.Serialize(new GetTaskRangeResponse
            {
                status_code = 0,
                tasks = Core.GetUserTasks(DateTime.UnixEpoch.AddSeconds(fromDate),
                    DateTime.UnixEpoch.AddSeconds(toDate), GetUserId())
            });
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }
    }

    [HttpGet("get-excel-report/{fromDate}/{toDate}")]
    public IActionResult GetExcelFile(int fromDate, int toDate)
    {
        var excel = Core.GetExcelReport(DateTime.UnixEpoch.AddSeconds(fromDate), DateTime.UnixEpoch.AddSeconds(toDate),
            1);
        return File(excel.GetByte(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    private class GetTaskRangeResponse : Response
    {
        public List<Domain.Entities.Task> tasks { get; set; }
    }

    private class GetTaskResponse : Response
    {
        public Domain.Entities.Task Task { get; set; }
    }
}