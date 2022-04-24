#region

using System.Text.Json;
using GrpcService1.App.Handlers.Http.Projects.Validations;
using GrpcService1.Domain.Entities;
using GrpcService1.Domain.Errors;
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
        RecordProjectValidation body;
        try
        {
            body = DecodePayloadJson<RecordProjectValidation>();
        }
        catch (Exception e)
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store  current request data for ip blacklist analysis
            return InvalidRequestResponse;
        }

        switch (ModelState.IsValid)
        {
            case false:
                // @TODO A proper message must be returned to user for invalid data
                return "data validation failed";
        }

        try
        {
            Core.RecordProject(new Project
            {
                Description = body.description,
                Name = body.name
            }, new User
            {
                Id = body.creator_id
            });
        }
        catch (Exception e)
        {
            return "internal error";
        }

        return "operation successful";
    }

    [HttpPost("edit-project")]
    public string UpdateProject()
    {
        UpdateProjectValidation body;
        try
        {
            body = DecodePayloadJson<UpdateProjectValidation>();
        }
        catch (Exception e)
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store  current request data for ip blacklist analysis
            return InvalidRequestResponse;
        }

        switch (ModelState.IsValid)
        {
            case false:
                // @TODO A proper error must be returned to user for invalid data
                return "data validation failed";
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
        catch (Exception e)
        {
            return "internal error occurred";
        }

        return "operation successful";
    }

    [HttpGet("get-project/{id}")]
    public string GetProject(int id)
    {
        try
        {
            var project = Core.GetProject(new Project
            {
                Id = id
            });

            return JsonSerializer.Serialize(new GetProjectResponse
            {
                StatusCode = 0,
                Project = project
            });
        }
        catch (Exception e)
        {
            return JsonSerializer.Serialize(new GetProjectResponse
            {
                StatusCode = 1
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
        catch (Exception e)
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store  current request data for ip blacklist analysis
            return InvalidRequestResponse;
        }

        switch (ModelState.IsValid)
        {
            case false:
                // @TODO A proper error must returned to user for invalid data 
                return "data validation failed";
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
        catch (Exception e)
        {
            return "Internal error occurred";
        }

        return "operation successful";
    }

    [HttpPost("remove-member")]
    public string RemoveMember()
    {
        RemoveMemberValidation body;
        try
        {
            body = DecodePayloadJson<RemoveMemberValidation>();
        }
        catch (Exception e)
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store  current request data for ip blacklist analysis
            return InvalidRequestResponse;
        }

        Console.WriteLine(body.project_id);
        Console.WriteLine(body.user_id);

        switch (ModelState.IsValid)
        {
            case false:
                // @TODO A proper error must be returned for invalid data
                return "data validation failed";
        }

        try
        {
            Core.RemoveMember(new ProjectMember
            {
                ProjectId = body.project_id,
                UserId = body.user_id
            });
        }
        catch (Exception e)
        {
            // @TODO A proper error must be returned to user for invalid data
            return "Internal error occurred";
        }

        return "operation successful";
    }

    [HttpPost("delete-project")]
    public string DeleteProject()
    {
        DeleteProjectValidation body;
        try
        {
            body = DecodePayloadJson<DeleteProjectValidation>();
        }
        catch (Exception e)
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store  current request data for ip blacklist analysis
            return InvalidRequestResponse;
        }

        switch (ModelState.IsValid)
        {
            case false:
                // @TODO A proper error must be returned for invalid data
                return "data validation failed";
        }

        try
        {
            Core.DeleteProject(new Project
            {
                Id = body.project_id
            });
        }
        catch (Exception e)
        {
            // @TODO A proper error must be returned to user
            return "operation failed";
        }

        return "operation successful";
    }

    [HttpPost("edit-member")]
    public string EditProjectMember()
    {
        AddMemberValidation body;
        try
        {
            body = DecodePayloadJson<AddMemberValidation>();
        }
        catch (Exception e)
        {
            // @TODO Since this exception happens when client is sending invalid data, we can store  current request data for ip blacklist analysis
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
            Core.UpdateProjectMember(new ProjectMember
            {
                Level = body.level,
                ProjectId = body.project_id,
                UserId = body.user_id
            });
        }
        catch (Exception e)
        {
            // @TODO A proper error must be be returned to client for because of internal failure
            return "operation failed";
        }

        return "operation successful";
    }

    private class GetProjectResponse
    {
        public int StatusCode { get; set; }
        public Project Project { get; set; }
    }
}