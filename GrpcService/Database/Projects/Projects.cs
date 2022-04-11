using GrpcService1.App.Core.Projects;
using GrpcService1.Domain.Entities;
using GrpcService1.ErrorReporter;

namespace GrpcService1.Database.Projects;

public class Projects : IDatabase
{
    private Connection Connection;
    private IErrorReporter ErrorReporter;

    public Projects(GrpcService1.Database.Projects.Connection connection, IErrorReporter errorReporter)
    {
        Connection = connection;
        ErrorReporter = errorReporter;
    }

    public IRecordProjectBatch RecordProject(Project project)
    {
        throw new NotImplementedException();
    }

    public void UpdateProject(Project project)
    {
        throw new NotImplementedException();
    }

    public void AddMemberToProject(Project project, User user)
    {
        throw new NotImplementedException();
    }

    public void RemoveUserFromProject(Project project, User user)
    {
        throw new NotImplementedException();
    }

    public void DeleteProject(Project project)
    {
        throw new NotImplementedException();
    }
}