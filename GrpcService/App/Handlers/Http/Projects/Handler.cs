using System.Text.Json;
using GrpcService1.App.Handlers.Http.Projects.Validations;
using GrpcService1.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GrpcService1.App.Handlers.Http.Projects;

[Route("/projects")]
public class Handler : BaseHandler
{
    private Core.Projects.Core Core;

    public Handler(Core.Projects.Core core)
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
            Core.RecordProject(new Project()
            {
                Description = body.description,
                Name = body.name,
            }, new User()
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
            Core.UpdateProject(new Project()
            {
                Id = body.project_id,
                Description = body.description,
                Name = body.name,
            });
        }
        catch (Exception e)
        {
            return "internal error occurred";
        }

        return "operation successful";
    }

    private class GetProjectResponse
    {
        public int StatusCode { get; set; }
        public Domain.Entities.Project Project { get; set; }
    }

    [HttpGet("get-project/{id}")]
    public string GetProject(int id)
    {
        try
        {
            var project = Core.GetProject(new Project()
            {
                Id = id,
            });

            return JsonSerializer.Serialize(new GetProjectResponse()
            {
                StatusCode = 0,
                Project = project
            });
        }
        catch (Exception e)
        {
            return JsonSerializer.Serialize(new GetProjectResponse()
            {
                StatusCode = 1,
            });
        }
    }

    public string AddMember([FromForm] int user_id, [FromForm] int project_id, [FromForm] string level)
    {
        AddMemberValidation validation = new AddMemberValidation()
        {
            ProjectId = project_id,
            UserId = user_id,
            Level = level,
        };

        switch (ModelState.IsValid)
        {
            case false:
                // @TODO A proper error must returned to user for invalid data 
                return "data validation failed";
        }

        try
        {
            Core.AddMember(new ProjectMember()
            {
                ProjectId = project_id,
                UserId = user_id,
                Level = level,
            });
        }
        catch (Exception e)
        {
            return "Internal error occurred";
        }

        return "operation successful";
    }

    public string RemoveMember([FromForm] int project_id, [FromForm] int user_id)
    {
        RemoveMemberValidation validation = new RemoveMemberValidation()
        {
            ProjectId = project_id,
            UserId = user_id,
        };

        switch (ModelState.IsValid)
        {
            case false:
                // @TODO A proper error must be returned for invalid data
                return "data validation failed";
        }

        try
        {
            Core.RemoveMember(new ProjectMember()
            {
                ProjectId = validation.ProjectId,
                UserId = validation.UserId,
            });
        }
        catch (Exception e)
        {
            // @TODO A proper error must be returned to user for invalid data
            return "Internal error occurred";
        }

        return "operation successful";
    }

    public string DeleteProject([FromForm] int project_id)
    {
        DeleteMemberValidation validation = new DeleteMemberValidation()
        {
            ProjectId = project_id,
        };

        switch (ModelState.IsValid)
        {
            case false:
                // @TODO A proper error must be returned for invalid data
                return "data validation failed";
        }

        try
        {
            Core.DeleteProject(new Project()
            {
                Id = validation.ProjectId,
            });
        }
        catch (Exception e)
        {
            // @TODO A proper error must be returned to user
            return "operation failed";
        }

        return "operation successful";
    }
}