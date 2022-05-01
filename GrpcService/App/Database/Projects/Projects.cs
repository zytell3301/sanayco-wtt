#region

using ErrorReporter;
using GrpcService1.App.Core.Projects;
using GrpcService1.App.Database.Model;
using GrpcService1.Domain.Errors;
using Project = GrpcService1.Domain.Entities.Project;
using ProjectMember = GrpcService1.Domain.Entities.ProjectMember;

#endregion

namespace GrpcService1.App.Database.Projects;

public class Projects : IDatabase
{
    private readonly wttContext Connection;
    private readonly IErrorReporter ErrorReporter;

    public Projects(wttContext connection, IErrorReporter errorReporter)
    {
        Connection = connection;
        ErrorReporter = errorReporter;
    }

    public IRecordProjectBatch RecordProject(Project project)
    {
        return new RecordProjectBatch(Connection, ErrorReporter, new Model.Project
        {
            Description = project.Description,
            Name = project.Name,
            CreatedAt = project.CreatedAt,
        });
    }

    public void UpdateProject(Project project)
    {
        var model = Connection.Projects.First(p => p.Id == project.Id);
        try
        {
            model.Description = project.Description;
            model.Name = project.Name;
            Connection.Projects.Update(model);
            Connection.SaveChanges();
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
            Connection.ProjectMembers.Add(new Model.ProjectMember
            {
                Level = projectMember.Level,
                ProjectId = projectMember.ProjectId,
                UserId = projectMember.UserId,
                CreatedAt = projectMember.CreatedAt,
            });
            Connection.SaveChanges();
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
            var model = Connection.ProjectMembers.Where(p => p.UserId == projectMember.UserId)
                .First(p => p.ProjectId == projectMember.ProjectId);
            Connection.ProjectMembers.Remove(model);
            Connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw new InternalError("");
        }
    }

    public void DeleteProject(Project project)
    {
        try
        {
            var model = Connection.Projects.First(p => p.Id == project.Id);
            Connection.Projects.Remove(model);
            Connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw new InternalError("");
        }
    }

    public void UpdateProjectMember(ProjectMember projectMember)
    {
        try
        {
            var model = Connection.ProjectMembers.Where(m => m.UserId == projectMember.UserId)
                .First(m => m.ProjectId == projectMember.ProjectId);
            model.Level = projectMember.Level;
            Connection.Update(model);
            Connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw new InternalError("");
        }
    }

    public Project GetProject(Project project)
    {
        try
        {
            return ConvertModelToProject(Connection.Projects.First(p => p.Id == project.Id));
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw new InternalError("");
        }
    }

    public ProjectMember GetProjectMember(ProjectMember member)
    {
        try
        {
            var model = Connection.ProjectMembers.Where(m => m.UserId == member.UserId)
                .First(m => m.ProjectId == member.ProjectId);
            return ConvertModelToProjectMember(model);
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw new InternalError("");
        }
    }

    private ProjectMember ConvertModelToProjectMember(Model.ProjectMember model)
    {
        return new ProjectMember()
        {
            Id = model.Id,
            Level = model.Level,
            CreatedAt = model.CreatedAt.Value,
            ProjectId = model.ProjectId.Value,
            UserId = model.UserId.Value,
        };
    }

    private Project ConvertModelToProject(Model.Project model)
    {
        return new Project
        {
            Id = model.Id,
            Description = model.Description,
            Name = model.Name,
            CreatedAt = model.CreatedAt.Value,
        };
    }
}