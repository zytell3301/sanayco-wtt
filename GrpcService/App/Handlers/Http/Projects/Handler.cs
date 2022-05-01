#region

using System.Text.Json;
using GrpcService1.App.Handlers.Http.Projects.Validations;
using GrpcService1.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace GrpcService1.App.Handlers.Http.Projects;

[Route("/projects")]
public class Handler : BaseHandler
{
    private readonly Core.Projects.Core Core;

    public Handler(Core.Projects.Core core, BaseHandlerDependencies baseHandlerDependencies) : base(
        baseHandlerDependencies)
    {
        Core = core;
    }

    [Route("record-project")]
    public string RecordProject()
    {
        try
        {
            Authorize("create-project");
        }
        catch (Exception)
        {
            return ResponseToJson(AuthorizationFailedResponse());
        }

        RecordProjectValidation body;
        try
        {
            body = DecodePayloadJson<RecordProjectValidation>();
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
            Core.RecordProject(new Project
            {
                Description = body.description,
                Name = body.name
            }, new User
            {
                Id = GetUserId()
            });
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }

    [HttpPost("edit-project")]
    public string UpdateProject()
    {
        UpdateProjectValidation body;
        try
        {
            body = DecodePayloadJson<UpdateProjectValidation>();
        }
        catch (Exception)
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store  current request data for ip blacklist analysis
            return InvalidRequestResponse;
        }

        try
        {
            switch (Core.CheckProjectOwnership(body.project_id, GetUserId()))
            {
                case false:
                    Authorize("edit-project");
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
            Core.UpdateProject(new Project
            {
                Id = body.project_id,
                Description = body.description,
                Name = body.name
            });
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }

    [HttpGet("get-project/{id}")]
    public string GetProject(int id)
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
            var project = Core.GetProject(new Project
            {
                Id = id
            });

            return JsonSerializer.Serialize(new GetProjectResponse
            {
                status_code = 0,
                Project = project
            });
        }
        catch (Exception)
        {
            return JsonSerializer.Serialize(new GetProjectResponse
            {
                status_code = 1
            });
        }
    }

    [HttpPost("add-member")]
    public string AddMember()
    {
        AddMemberValidation body;
        try
        {
            body = DecodePayloadJson<AddMemberValidation>();
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
            switch (Core.CheckProjectOwnership(body.project_id, GetUserId()))
            {
                case false:
                    Authorize("add-project-member");
                    break;
            }
        }
        catch (Exception)
        {
            return ResponseToJson(AuthorizationFailedResponse());
        }


        try
        {
            Core.AddMember(new ProjectMember
            {
                ProjectId = body.project_id,
                UserId = body.user_id,
                Level = body.level
            });
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }

    [HttpPost("remove-member")]
    public string RemoveMember()
    {
        RemoveMemberValidation body;
        try
        {
            body = DecodePayloadJson<RemoveMemberValidation>();
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
            switch (Core.CheckProjectOwnership(body.project_id, GetUserId()))
            {
                case false:
                    Authorize("delete-project-member");
                    break;
            }
        }
        catch (Exception)
        {
            return ResponseToJson(AuthorizationFailedResponse());
        }

        try
        {
            Core.RemoveMember(new ProjectMember
            {
                ProjectId = body.project_id,
                UserId = body.user_id
            });
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }

    [HttpPost("delete-project")]
    public string DeleteProject()
    {
        DeleteProjectValidation body;
        try
        {
            body = DecodePayloadJson<DeleteProjectValidation>();
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
            switch (Core.CheckProjectOwnership(body.project_id, GetUserId()))
            {
                case false:
                    Authorize("delete-project");
                    break;
            }
        }
        catch (Exception)
        {
            return ResponseToJson(AuthorizationFailedResponse());
        }

        try
        {
            Core.DeleteProject(new Project
            {
                Id = body.project_id
            });
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }

    [HttpPost("edit-member")]
    public string EditProjectMember()
    {
        AddMemberValidation body;
        try
        {
            body = DecodePayloadJson<AddMemberValidation>();
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
            switch (Core.CheckProjectOwnership(body.project_id, GetUserId()))
            {
                case false:
                    Authorize("edit-project-member");
                    break;
            }
        }
        catch (Exception)
        {
            return ResponseToJson(AuthorizationFailedResponse());
        }

        try
        {
            Core.UpdateProjectMember(new ProjectMember
            {
                Level = body.level,
                ProjectId = body.project_id,
                UserId = body.user_id
            });
        }
        catch (Exception)
        {
            return ResponseToJson(InternalErrorResponse());
        }

        return ResponseToJson(OperationSuccessfulResponse());
    }

    private class GetProjectResponse : Response
    {
        public Project Project { get; set; }
    }
}