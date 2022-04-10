using GrpcService1.Domain.Entities;
using GrpcService1.Domain.Errors;

namespace GrpcService1.App.Core.Projects;

public class Core
{
    private InternalError InternalError;
    private OperationSuccessful OperationSuccessful;
    private IDatabase Database;

    public class ProjectsCoreConfigs
    {
        public string InternalErrorMessage;
        public string OperationSuccessfulMessage;
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

    public Status RecordProject(Project project)
    {
        try
        {
            Database.RecordProject(project);
        }
        catch (Exception e)
        {
            return InternalError;
        }

        return OperationSuccessful;
    }

    public Status UpdateProject(Project project)
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
}