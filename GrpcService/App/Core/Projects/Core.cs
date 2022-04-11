#region

using GrpcService1.Domain.Entities;
using GrpcService1.Domain.Errors;

#endregion

namespace GrpcService1.App.Core.Projects;

public class Core
{
    private string CreatorProjectMemberCode;

    private readonly IDatabase Database;
    private readonly InternalError InternalError;
    private readonly OperationSuccessful OperationSuccessful;

    public Core(ProjectsCoreDependencies dependencies, ProjectsCoreConfigs configs)
    {
        InternalError = new InternalError(configs.InternalErrorMessage);
        OperationSuccessful = new OperationSuccessful(configs.OperationSuccessfulMessage);

        Database = dependencies.Database;
    }

    public Domain.Errors.Status RecordProject(Project project, User creator)
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
            return InternalError;
        }

        return OperationSuccessful;
    }

    public Domain.Errors.Status UpdateProject(Project project)
    {
        try
        {
            Database.UpdateProject(project);
        }
        catch (Exception e)
        {
            return InternalError;
        }

        return OperationSuccessful;
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

    public Domain.Errors.Status RemoveMember(Project project, User user)
    {
        try
        {
            Database.RemoveUserFromProject(project, user);
        }
        catch (Exception e)
        {
            return InternalError;
        }

        return OperationSuccessful;
    }

    public Domain.Errors.Status DeleteProject(Project project)
    {
        try
        {
            Database.DeleteProject(project);
        }
        catch (Exception e)
        {
            return InternalError;
        }

        return OperationSuccessful;
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