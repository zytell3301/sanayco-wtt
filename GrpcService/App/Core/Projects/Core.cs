#region

using GrpcService1.Domain.Entities;
using GrpcService1.Domain.Errors;

#endregion

namespace GrpcService1.App.Core.Projects;

public class Core
{
    private readonly string CreatorProjectMemberCode;
    private readonly IDatabase Database;
    private readonly InternalError InternalError;
    private readonly OperationSuccessful OperationSuccessful;

    public Core(ProjectsCoreDependencies dependencies, ProjectsCoreConfigs configs)
    {
        InternalError = new InternalError(configs.InternalErrorMessage);
        OperationSuccessful = new OperationSuccessful(configs.OperationSuccessfulMessage);
        CreatorProjectMemberCode = configs.CreatorProjectMemberCode;

        Database = dependencies.Database;
    }

    public bool CheckProjectOwnership(int projectId, int userId)
    {
        var member = Database.GetProjectMember(new ProjectMember
        {
            ProjectId = projectId,
            UserId = userId
        });
        return CheckMemberAccess(member.Level);
    }

    // For now the access for modifying a project is only granted to its creator. Change this function if needed
    private bool CheckMemberAccess(string level)
    {
        return level == CreatorProjectMemberCode;
    }

    public void RecordProject(Project project, User creator)
    {
        try
        {
            project.CreatedAt = DateTime.Now;
            var batch = Database.RecordProject(project);
            /*
             * Since every user that creates a project is a member of it, so we must
             * add the creator as the creator role of project to database
             */
            var member = new ProjectMember();
            member.Level = CreatorProjectMemberCode;
            member.UserId = creator.Id;
            member.CreatedAt = DateTime.Now;
            batch.AddProjectMember(member);
            batch.ExecuteOperation();
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public Project GetProject(Project project)
    {
        try
        {
            return Database.GetProject(project);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public void UpdateProject(Project project)
    {
        try
        {
            Database.UpdateProject(project);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public Domain.Errors.Status AddMember(ProjectMember projectMember)
    {
        try
        {
            projectMember.CreatedAt = DateTime.Now;
            Database.AddMemberToProject(projectMember);
        }
        catch (EntityNotFound)
        {
            return new EntityNotFound("User not found");
        }
        catch (Exception)
        {
            return InternalError;
        }

        return OperationSuccessful;
    }

    public void RemoveMember(ProjectMember projectMember)
    {
        try
        {
            Database.RemoveUserFromProject(projectMember);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public void DeleteProject(Project project)
    {
        try
        {
            Database.DeleteProject(project);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public void UpdateProjectMember(ProjectMember projectMember)
    {
        try
        {
            Database.UpdateProjectMember(projectMember);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public class ProjectsCoreConfigs
    {
        public string CreatorProjectMemberCode;
        public string InternalErrorMessage;
        public string OperationSuccessfulMessage;
    }

    public class ProjectsCoreDependencies
    {
        public IDatabase Database;
    }
}