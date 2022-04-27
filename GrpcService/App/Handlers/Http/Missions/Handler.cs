#region

using GrpcService1.App.Handlers.Http.Missions.Validations;
using GrpcService1.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace GrpcService1.App.Handlers.Http.Missions;

[Route("/missions")]
public class Handler : BaseHandler
{
    private readonly Core.Missions.Core Core;

    public Handler(Core.Missions.Core core, BaseHandlerDependencies baseHandlerDependencies) : base(
        baseHandlerDependencies)
    {
        Core = core;
    }

    [HttpPost("record-mission")]
    public string RecordMission()
    {
        try
        {
            Authorize("submit-mission");
        }
        catch (Exception)
        {
            return ResponseToJson(AuthorizationFailedResponse());
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
            Core.RecordMission(new Mission
            {
                Location = body.location,
                Description = body.description,
                Title = body.title,
                FromDate = DateTime.UnixEpoch.AddSeconds(body.from_date),
                IsVerified = body.is_verified,
                MemberId = GetUserId(),
                ToDate = DateTime.UnixEpoch.AddSeconds(body.to_date),
                ProjectId = body.project_id
            });
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }

    [HttpPost("delete-mission")]
    public string DeleteMission()
    {
        try
        {
            // @TODO We must check that if the requesting client is the owner of the current entity
        }
        catch (Exception)
        {
            return ResponseToJson(AuthorizationFailedResponse());
        }

        DeleteMissionValidation body;
        try
        {
            body = DecodePayloadJson<DeleteMissionValidation>();
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
            Core.DeleteMission(new Mission
            {
                Id = body.mission_id
            });
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }

    public string UpdateMission()
    {
        try
        {
            // @TODO We must check that if the requesting client is the owner of current entity or not
        }
        catch (Exception)
        {
            return ResponseToJson(AuthorizationFailedResponse());
        }

        UpdateMissionValidation body;
        try
        {
            body = DecodePayloadJson<UpdateMissionValidation>();
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
            Core.UpdateMission(new Mission
            {
                Id = body.mission_id,
                Description = body.description,
                Location = body.location,
                FromDate = DateTime.UnixEpoch.AddSeconds(body.from_date),
                ToDate = DateTime.UnixEpoch.AddSeconds(body.to_date),
                Title = body.title,
                ProjectId = body.project_id
            });
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }
}