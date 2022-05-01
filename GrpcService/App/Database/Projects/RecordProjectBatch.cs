#region

using ErrorReporter;
using GrpcService1.App.Core.Projects;
using GrpcService1.App.Database.Model;
using GrpcService1.Domain.Errors;
using ProjectMember = GrpcService1.Domain.Entities.ProjectMember;

#endregion

namespace GrpcService1.App.Database.Projects;

public class RecordProjectBatch : IRecordProjectBatch
{
    private readonly wttContext Connection;
    private readonly IErrorReporter ErrorReporter;
    private readonly Project Project;

    public RecordProjectBatch(wttContext connection, IErrorReporter errorReporter,
        Project project)
    {
        Project = project;
        Connection = connection;
        ErrorReporter = errorReporter;
        try
        {
            Connection.Projects.Add(Project);
            connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw new InternalError("");
        }
    }

    public void AddProjectMember(ProjectMember projectMember)
    {
        try
        {
            Connection.ProjectMembers.Add(new Model.ProjectMember
            {
                Level = projectMember.Level,
                ProjectId = Project.Id,
                UserId = projectMember.UserId,
                CreatedAt = projectMember.CreatedAt,
            });
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw new InternalError("");
        }
    }

    public void ExecuteOperation()
    {
        try
        {
            Connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw new InternalError("");
        }
    }
}