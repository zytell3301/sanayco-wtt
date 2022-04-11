using GrpcService1.Domain.Entities;
using GrpcService1.Domain.Errors;

namespace GrpcService1.App.Core.Projects;

public class Core
{
    private InternalError InternalError;
    private OperationSuccessful OperationSuccessful;
    private string CreatorProjectMemberCode;

    private IDatabase Database;

    public class ProjectsCoreConfigs
    {
        public string InternalErrorMessage;
        public string OperationSuccessfulMessage;

        public string CreatorProjectMemberCode;
    }

    public class ProjectsCoreDependencies
    {
        public IDatabase Database;
    }

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
            member.MemberLevel = CreatorProjectMemberCode;
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

    public Domain.Errors.Status AddMember(Project project, User user)
    {
        try
        {
            Database.AddMemberToProject(project, user);
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
}