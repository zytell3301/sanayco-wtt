#region

using GrpcService1.Domain.Entities;

#endregion

namespace GrpcService1.App.Core.Projects;

public interface IDatabase
{
    public IRecordProjectBatch RecordProject(Project project);
    public void UpdateProject(Project project);
    public void AddMemberToProject(ProjectMember projectMember);
    public void RemoveUserFromProject(ProjectMember projectMember);
    public void DeleteProject(Project project);
    public void UpdateProjectMember(ProjectMember projectMember);
    public Project GetProject(Project project);
    public ProjectMember GetProjectMember(ProjectMember member);
    public List<Project> SearchForProject(string title);
}

/**
 * Since two records are being inserted into database and they are dependent to each other,
 * we can move it to an interface like the one below to first avoid the database layer for doing
 * the business logic and second avoid inconsistency of database.
 * 
 * If the process of creating a batch is not directly part of business logic, this process must
 * be moved to an intermediate layer called DatabaseController that is interacting with core and the
 * underlying database layer to isolate the batching process with database interaction process.
 */
public interface IRecordProjectBatch
{
    public void AddProjectMember(ProjectMember projectMember);
    public void ExecuteOperation();
}