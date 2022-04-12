using GrpcService1.App.Core.Projects;
using GrpcService1.Domain.Entities;
using GrpcService1.Domain.Errors;
using GrpcService1.ErrorReporter;

namespace GrpcService1.Database.Projects;

public class RecordProjectBatch : IRecordProjectBatch
{
    private GrpcService1.Database.Projects.Connection Connection;
    private IErrorReporter ErrorReporter;

    public RecordProjectBatch(GrpcService1.Database.Projects.Connection connection, IErrorReporter errorReporter,
        Project project)
    {
        Connection = connection;
        ErrorReporter = errorReporter;
        try
        {
            Connection.Projects.Add(project);
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