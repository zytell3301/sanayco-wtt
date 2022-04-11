using GrpcService1.App.Core.Projects;
using GrpcService1.Domain.Entities;
using GrpcService1.Domain.Errors;
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
        try
        {
            Connection.Projects.Update(new Project()
            {
                Description = project.Description,
                Name = project.Name,
            });
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw new InternalError("");
        }
    }

    public void AddMemberToProject(ProjectMember projectMember)
    {
        try
        {
            Connection.ProjectMembers.Add(new ProjectMember()
            {
                Level = projectMember.Level,
                ProjectId = projectMember.ProjectId,
                UserId = projectMember.UserId,
            });
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw new InternalError("");
        }
    }

    public void RemoveUserFromProject(ProjectMember projectMember)
    {
        try
        {
            Connection.ProjectMembers.Remove(new ProjectMember()
            {
                Id = projectMember.Id
            });
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw new InternalError("");
        }
    }

    public void DeleteProject(Project project)
    {
        throw new NotImplementedException();
    }
}