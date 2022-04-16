using GrpcService1.App.Handlers.Http.OffTime.Validations;
using GrpcService1.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GrpcService1.App.Handlers.Http.OffTime;

public class Handler : ControllerBase
{
    private Core.OffTime.Core Core;

    public Handler(Core.OffTime.Core core)
    {
        Core = core;
    }

    public string RecordOffTime([FromForm] int from_date, [FromForm] int to_date, [FromForm] string description,
        [FromForm] int user_id)
    {
        RecordOffTimeValidation validation = new RecordOffTimeValidation()
        {
            Description = description,
            FromDate = from_date,
            ToDate = to_date,
            UserId = user_id,
        };

        switch (ModelState.IsValid)
        {
            case false:
                // @TODO A proper error must be returned to user because of invalid data
                return "data validation failed";
        }

        try
        {
            Core.RecordOffTime(new User(){Id = validation.UserId},new Domain.Entities.OffTime()
            {
                Description = validation.Description,
                FromDate = DateTime.UnixEpoch.AddSeconds(validation.FromDate),
                ToDate = DateTime.UnixEpoch.AddSeconds(validation.ToDate),
                UserId = validation.UserId,
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