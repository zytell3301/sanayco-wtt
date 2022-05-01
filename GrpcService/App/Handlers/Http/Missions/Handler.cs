#region

using System.Text.Json;
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
            switch (Core.CheckMissionOwnership(body.mission_id, GetUserId()))
            {
                case false:
                    Authorize("delete-mission");
                    break;
            }
        }
        catch (Exception)
        {
            return ResponseToJson(AuthorizationFailedResponse());
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

    [HttpGet("get-mission/{id}")]
    public string GetMission(int id)
    {
        try
        {
            // @TODO We must check that if the requesting client is the owner of current entity or not
        }
        catch (Exception)
        {
            return ResponseToJson(AuthorizationFailedResponse());
        }

        try
        {
            var mission = Core.GetMission(new Mission
            {
                Id = id
            });
            return JsonSerializer.Serialize(new GetMissionResponse
            {
                status_code = 0,
                mission = new GetMissionResponse.Mission
                {
                    id = mission.Id,
                    description = mission.Description,
                    from_date = mission.FromDate,
                    is_verified = mission.IsVerified,
                    location = mission.Location,
                    member_id = mission.MemberId,
                    project_id = mission.ProjectId,
                    title = mission.Title,
                    to_date = mission.ToDate
                }
            });
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }
    }

    [HttpPost("update-mission")]
    public string UpdateMission()
    {
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
            switch (Core.CheckMissionOwnership(body.mission_id, GetUserId()))
            {
                case false:
                    Authorize("edit-mission");
                    break;
            }
        }
        catch (Exception)
        {
            return ResponseToJson(AuthorizationFailedResponse());
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

    [HttpPost("approve")]
    public string ApproveMission()
    {
        try
        {
            Authorize("change-mission-status");
        }
        catch (Exception)
        {
            return ResponseToJson(AuthorizationFailedResponse());
        }

        ChangeMissionStatusValidation body;
        try
        {
            body = DecodePayloadJson<ChangeMissionStatusValidation>();
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
            Core.ApproveMission(new Mission
            {
                Id = body.mission_id,
                IsVerified = true
            });
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }

    private class GetMissionResponse : Response
    {
        public Mission mission { get; set; }

        public class Mission
        {
            public int id { get; set; }
            public int member_id { get; set; }
            public int project_id { get; set; }
            public DateTime from_date { get; set; }
            public DateTime to_date { get; set; }
            public string description { get; set; }
            public string title { get; set; }
            public string location { get; set; }
            public bool is_verified { get; set; }
        }
    }
}