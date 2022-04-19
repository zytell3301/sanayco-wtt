using System.Text.Json;
using GrpcService1.App.Handlers.Http.OffTime.Validations;
using GrpcService1.App.Handlers.Http.tasks.Validations;
using GrpcService1.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GrpcService1.App.Handlers.Http.OffTime;

[Route("off-times")]
public class Handler : BaseHandler
{
    private Core.OffTime.Core Core;

    public Handler(Core.OffTime.Core core)
    {
        Core = core;
    }

    [HttpPost("record-off-time")]
    public string RecordOffTime()
    {
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
                // @TODO A proper error must be returned to user because of invalid data
                return "data validation failed";
        }

        try
        {
            Core.RecordOffTime(new User() {Id = body.user_id}, new Domain.Entities.OffTime()
            {
                Description = body.description,
                FromDate = DateTime.UnixEpoch.AddSeconds(body.from_date),
                ToDate = DateTime.UnixEpoch.AddSeconds(body.to_date),
                UserId = body.user_id,
            });
        }
        catch (Exception e)
        {
            // @TODO A proper error must be returned to client because of internal failure
            return "operation failed";
        }

        return "operation successful";
    }

    [HttpPost("approve-off-time")]
    public string ApproveOffTime()
    {
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
                // @TODO A proper error must be returned to client for invalid data
                return "data validation failed";
        }

        try
        {
            Core.ApproveOffTime(new Domain.Entities.OffTime()
            {
                Id = body.off_time_id,
            });
        }
        catch (Exception e)
        {
            // @TODO A proper error must be returned to user because of internal failure
            return "operation failed";
        }

        return "operation successful";
    }

    [HttpPost("reject-off-time")]
    public string RejectOffTime()
    {
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
                // @TODO A proper error must be returned to client because of invalid data
                return "data validation failed";
        }

        try
        {
            Core.RejectOffTime(new Domain.Entities.OffTime()
            {
                Id = body.off_time_id,
            });
        }
        catch (Exception e)
        {
            // @TODO A proper error must be returned to client because of internal failure
            return "operation failed";
        }

        return "operation successful";
    }

    [HttpPost("set-off-time-waiting")]
    public string SetOffTimeWaiting()
    {
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
                // @TODO A proper error must be returned to client for invalid data
                return "data validation failed";
        }

        try
        {
            Core.SetOffTimeStatusWaiting(new Domain.Entities.OffTime()
            {
                Id = body.off_time_id,
            });
        }
        catch (Exception e)
        {
            // @TODO A proper error must be returned to client for internal failure
            return "operation failed";
        }

        return "operation successful";
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
                // @TODO A proper error must be returned to client for invalid data
                return "data validation failed";
        }

        try
        {
            Core.CancelOffTime(new Domain.Entities.OffTime()
            {
                Id = body.off_time_id,
            });
        }
        catch (Exception e)
        {
            // @TODO A proper error must be returned to client for internal failure
            return "operation failed";
        }

        return "operation successful";
    }

    private class GetOffTimeResponse
    {
        public int StatusCode { get; set; }
        public Domain.Entities.OffTime OffTime { get; set; }
    }

    [HttpGet("get-off-time/{id}")]
    public string GetOffTime(int id)
    {
        Domain.Entities.OffTime offTime = new Domain.Entities.OffTime()
        {
            Id = id,
        };

        try
        {
            offTime = Core.GetOffTime(offTime);
        }
        catch (Exception e)
        {
            return JsonSerializer.Serialize(new GetOffTimeResponse()
            {
                StatusCode = 1,
            });
        }

        return JsonSerializer.Serialize(new GetOffTimeResponse()
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
                // @TODO A proper error must be returned to user for invalid data
                return "operation failed";
        }

        try
        {
            Core.EditOffTime(new Domain.Entities.OffTime()
            {
                Id = body.off_time_id,
                Description = body.description,
                FromDate = DateTime.UnixEpoch.AddSeconds(body.from_date),
                ToDate = DateTime.UnixEpoch.AddSeconds(body.to_date),
            });
        }
        catch (Exception e)
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store current request data for ip blacklist analysis
            return "operation failed";
        }

        return "operation successful";
    }
}