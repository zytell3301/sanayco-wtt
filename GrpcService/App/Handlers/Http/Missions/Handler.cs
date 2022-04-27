using GrpcService1.App.Handlers.Http.Missions.Validations;
using GrpcService1.Domain.Entities;
using GrpcService1.Domain.Errors;
using Microsoft.AspNetCore.Mvc;

namespace GrpcService1.App.Handlers.Http.Missions;

[Route("/missions")]
public class Handler : BaseHandler
{
    private Core.Missions.Core Core;

    public Handler(Core.Missions.Core core, BaseHandlerDependencies baseHandlerDependencies) : base(
        baseHandlerDependencies)
    {
        Core = core;
    }

    [HttpPost("record-missions")]
    public string RecordMission()
    {
        try
        {
            Authorize("mission");
        }
        catch (Exception)
        {
            return ResponseToJson(AuthenticationFailedResponse());
        }

        RecordMissionsValidation body;
        try
        {
            body = DecodePayloadJson<RecordMissionsValidation>();
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
            Core.RecordMission(new Mission()
            {
                Location = body.location,
                Description = body.description,
                Title = body.title,
                FromDate = DateTime.UnixEpoch.AddSeconds(body.from_date),
                IsVerified = body.is_verified,
                MemberId = GetUserId(),
                ToDate = DateTime.UnixEpoch.AddSeconds(body.to_date),
                ProjectId = body.project_id,
            });
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }
}