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

    public void RecordProject(Project project, User creator)
    {
        try
        {
            var batch = Database.RecordProject(project);
            /*
             * Since every user that creates a project is a member of it, so we must
             * add the creator as the creator role of project to database
             */
            var member = new ProjectMember();
            member.Level = CreatorProjectMemberCode;
            member.UserId = creator.Id;
            batch.AddProjectMember(member);
            batch.ExecuteOperation();
        }
        catch (Exception e)
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
        catch (Exception e)
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
        catch (Exception e)
        {
            throw InternalError;
        }
    }

    public Domain.Errors.Status AddMember(ProjectMember projectMember)
    {
        try
        {
            Database.AddMemberToProject(projectMember);
        }
        catch (EntityNotFound e)
        {
            return new EntityNotFound("User not found");
        }
        catch (Exception e)
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
        catch (Exception e)
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
        catch (Exception e)
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
        catch (Exception e)
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