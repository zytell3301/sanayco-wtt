using GrpcService1.Domain.Entities;

namespace GrpcService1.App.Core.Projects;

public interface IDatabase
{
    public void RecordProject(Project project);
}