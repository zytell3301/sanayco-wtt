#region

using System.Text.Json;
using GrpcService1.App.Handlers.Http.OffTime.Validations;
using GrpcService1.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace GrpcService1.App.Handlers.Http.OffTime;

[Route("off-times")]
public class Handler : BaseHandler
{
    private readonly Core.OffTime.Core Core;

    public Handler(Core.OffTime.Core core, BaseHandlerDependencies baseHandlerDependencies) : base(
        baseHandlerDependencies)
    {
        Core = core;
    }

    [HttpPost("record-off-time")]
    public string RecordOffTime()
    {
        try
        {
            Authorize("record-off-time");
        }
        catch (Exception e)
        {
            return ResponseToJson(AuthorizationFailedResponse());
        }

        RecordOffTimeValidation body;
        try
        {
            body = DecodePayloadJson<RecordOffTimeValidation>();
        }
        catch (Exception e)
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store current request data for ip blacklist analysis
            return InvalidRequestResponse;
        }

        switch (ModelState.IsValid)
        {
            case false:
                return ResponseToJson(DataValidationFailedResponse());
        }

        try
        {
            Core.RecordOffTime(new User {Id = body.user_id}, new Domain.Entities.OffTime
            {
                Description = body.description,
                FromDate = DateTime.UnixEpoch.AddSeconds(body.from_date),
                ToDate = DateTime.UnixEpoch.AddSeconds(body.to_date),
                UserId = body.user_id
            });
        }
        catch (Exception e)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }

    [HttpPost("approve-off-time")]
    public string ApproveOffTime()
    {
        try
        {
            Authorize("change-off-time-status");
        }
        catch (Exception e)
        {
            return ResponseToJson(AuthorizationFailedResponse());
        }

        UpdateOffTimeStatusValidation body;
        try
        {
            body = DecodePayloadJson<UpdateOffTimeStatusValidation>();
        }
        catch (Exception e)
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store current request data for ip blacklist analysis
            return InvalidRequestResponse;
        }

        switch (ModelState.IsValid)
        {
            case false:
                return ResponseToJson(DataValidationFailedResponse());
        }

        try
        {
            Core.ApproveOffTime(new Domain.Entities.OffTime
            {
                Id = body.off_time_id
            });
        }
        catch (Exception e)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }

    [HttpPost("reject-off-time")]
    public string RejectOffTime()
    {
        try
        {
            Authorize("change-off-time-status");
        }
        catch (Exception e)
        {
            return ResponseToJson(AuthorizationFailedResponse());
        }

        UpdateOffTimeStatusValidation body;
        try
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store current request data for ip blacklist analysis
            body = DecodePayloadJson<UpdateOffTimeStatusValidation>();
        }
        catch (Exception e)
        {
            return InvalidRequestResponse;
        }

        switch (ModelState.IsValid)
        {
            case false:
                return ResponseToJson(DataValidationFailedResponse());
        }

        try
        {
            Core.RejectOffTime(new Domain.Entities.OffTime
            {
                Id = body.off_time_id
            });
        }
        catch (Exception e)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }

    [HttpPost("set-off-time-waiting")]
    public string SetOffTimeWaiting()
    {
        try
        {
            Authorize("change-off-time-status");
        }
        catch (Exception e)
        {
            return ResponseToJson(AuthorizationFailedResponse());
        }

        UpdateOffTimeStatusValidation body;
        try
        {
            body = DecodePayloadJson<UpdateOffTimeStatusValidation>();
        }
        catch (Exception e)
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store current request data for ip blacklist analysis
            return InvalidRequestResponse;
        }

        switch (ModelState.IsValid)
        {
            case false:
                return ResponseToJson(DataValidationFailedResponse());
        }

        try
        {
            Core.SetOffTimeStatusWaiting(new Domain.Entities.OffTime
            {
                Id = body.off_time_id
            });
        }
        catch (Exception e)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }

    [HttpPost("cancel-off-time")]
    public string CancelOffTime()
    {
        CancelOffTimeValidation body;
        try
        {
            body = DecodePayloadJson<CancelOffTimeValidation>();
        }
        catch (Exception e)
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store current request data for ip blacklist analysis
            return InvalidRequestResponse;
        }

        switch (ModelState.IsValid)
        {
            case false:
                return ResponseToJson(DataValidationFailedResponse());
        }

        try
        {
            Core.CancelOffTime(new Domain.Entities.OffTime
            {
                Id = body.off_time_id
            });
        }
        catch (Exception e)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }

    [HttpGet("get-off-time/{id}")]
    public string GetOffTime(int id)
    {
        var offTime = new Domain.Entities.OffTime
        {
            Id = id
        };

        try
        {
            offTime = Core.GetOffTime(offTime);
        }
        catch (Exception e)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return JsonSerializer.Serialize(new GetOffTimeResponse
        {
            StatusCode = 0,
            OffTime = offTime
        });
    }

    [HttpPost("edit-off-time")]
    public string EditOffTime()
    {
        EditOffTimeValidation body;
        try
        {
            body = DecodePayloadJson<EditOffTimeValidation>();
        }
        catch (Exception e)
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store current request data for ip blacklist analysis.
            return InvalidRequestResponse;
        }

        switch (ModelState.IsValid)
        {
            case false:
                return ResponseToJson(DataValidationFailedResponse());
        }

        try
        {
            Core.EditOffTime(new Domain.Entities.OffTime
            {
                Id = body.off_time_id,
                Description = body.description,
                FromDate = DateTime.UnixEpoch.AddSeconds(body.from_date),
                ToDate = DateTime.UnixEpoch.AddSeconds(body.to_date)
            });
        }
        catch (Exception e)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }

    private class GetOffTimeResponse
    {
        public int StatusCode { get; set; }
        public Domain.Entities.OffTime OffTime { get; set; }
    }
}