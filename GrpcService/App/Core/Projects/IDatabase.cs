using GrpcService1.Domain.Entities;

namespace GrpcService1.App.Core.Projects;

public interface IDatabase
{
    public void RecordProject(Project project);
    public void UpdateProject(Project project);
    public void AddMemberToProject(Project project, User user);
}