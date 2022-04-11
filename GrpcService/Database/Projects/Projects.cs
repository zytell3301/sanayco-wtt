using GrpcService1.App.Core.Projects;
using GrpcService1.Domain.Entities;

namespace GrpcService1.Database.Projects;

public class Projects : IDatabase
{
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