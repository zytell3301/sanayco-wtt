using GrpcService1.App.Core.Projects;
using GrpcService1.Domain.Entities;
using GrpcService1.Domain.Errors;
using ErrorReporter;

namespace GrpcService1.App.Database.Projects;

public class RecordProjectBatch : IRecordProjectBatch
{
    private Model.wttContext Connection;
    private IErrorReporter ErrorReporter;

    public RecordProjectBatch(Model.wttContext connection, IErrorReporter errorReporter,
        Model.Project project)
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

    public void AddProjectMember(Domain.Entities.ProjectMember projectMember)
    {
        try
        {
            Connection.ProjectMembers.Add(new Model.ProjectMember()
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