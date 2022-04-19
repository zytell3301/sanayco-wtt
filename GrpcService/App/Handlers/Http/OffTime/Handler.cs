using GrpcService1.App.Handlers.Http.OffTime.Validations;
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

    public string ApproveOffTime([FromForm] int off_time_id)
    {
        ApproveOffTimeValidation validation = new ApproveOffTimeValidation()
        {
            OffTimeId = off_time_id,
        };

        switch (ModelState.IsValid)
        {
            case true:
                // @TODO A proper error must be returned to client for invalid data
                return "data validation failed";
        }

        try
        {
        }
        catch (Exception e)
        {
            // @TODO A proper error must be returned to user because of internal failure
            return "operation failed";
        }

        return "operation successful";
    }

    public string RejectOffTime([FromForm] int off_time_id)
    {
        RejectOffTimeValidation validation = new RejectOffTimeValidation()
        {
            OffTimeId = off_time_id,
        };

        switch (ModelState.IsValid)
        {
            case true:
                // @TODO A proper error must be returned to client because of invalid data
                return "data validation failed";
        }

        try
        {
            Core.ApproveOffTime(new Domain.Entities.OffTime()
            {
                Id = validation.OffTimeId,
            });
        }
        catch (Exception e)
        {
            // @TODO A proper error must be returned to client because of internal failure
            return "operation failed";
        }

        return "operation successful";
    }
}