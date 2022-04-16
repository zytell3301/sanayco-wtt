using GrpcService1.App.Handlers.Http.Projects.Validations;
using GrpcService1.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GrpcService1.App.Handlers.Http.Projects;

[Route("/projects")]
public class Handler : ControllerBase
{
    private Core.Projects.Core Core;

    public Handler(Core.Projects.Core core)
    {
        Core = core;
    }

    [Route("new-project")]
    public string RecordProject([FromForm] string description, [FromForm] string name, [FromForm] int user_id)
    {
        RecordProjectValidation validation = new RecordProjectValidation()
        {
            Description = description,
            Name = name,
            UserId = user_id,
        };

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
                Description = validation.Description,
                Name = validation.Name,
            }, new User()
            {
                Id = validation.UserId
            });
        }
        catch (Exception e)
        {
            return "internal error";
        }

        return "operation successful";
    }

    public string UpdateProject([FromForm] string description, [FromForm] string name)
    {
        UpdateProjectValidation validation = new UpdateProjectValidation()
        {
            Description = description,
            Name = name,
        };

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
                Description = validation.Description,
                Name = validation.Name,
            });
        }
        catch (Exception e)
        {
            return "internal error occurred";
        }

        return "operation successful";
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