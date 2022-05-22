#region

using ErrorReporter;
using GrpcService1.App.Core.Presentation;
using GrpcService1.App.Database.Model;
using GrpcService1.Domain.Errors;
using Presentation = GrpcService1.Domain.Entities.Presentation;
using User = GrpcService1.Domain.Entities.User;

#endregion

namespace GrpcService1.App.Database.Presentations;

public class Presentations : IDatabase
{
    private readonly wttContext Connection;
    private readonly IErrorReporter ErrorReporter;
    private readonly InternalError InternalError;

    public class PresentationsDatabaseDependencies
    {
        public wttContext Connection;
        public IErrorReporter ErrorReporter;
        public InternalError InternalError;
    }

    public Presentations(PresentationsDatabaseDependencies dependencies)
    {
        Connection = dependencies.Connection;
        ErrorReporter = dependencies.ErrorReporter;

        InternalError = dependencies.InternalError;
    }

    public void RecordPresentation(User user)
    {
        try
        {
            Connection.Presentations.Add(new Model.Presentation
            {
                UserId = user.Id,
                Start = DateTime.Now
            });
            Connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
    }

    public void RecordPresentationEnd(User user)
    {
        try
        {
            Connection.Presentations.OrderBy(p => p.Id).Last(p => p.UserId == user.Id).End = DateTime.Now;
            Connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
    }

    public DateTime GetPresentationTime(User user)
    {
        try
        {
            return (DateTime) Connection.Presentations.Last(p => p.UserId == user.Id).Start;
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
    }

    public List<Presentation> GetPresentationsRange(DateTime fromDate, DateTime toDate, int userId)
    {
        var presentations = new List<Presentation>();
        try
        {
            foreach (var presentation in
                     Connection.Presentations.Where(p => p.Start > fromDate && p.End < toDate && p.UserId == userId)
                         .ToList())
                presentations.Add(new Presentation
                {
                    Id = presentation.Id,
                    End = presentation.End,
                    Start = presentation.Start,
                    UserId = presentation.UserId.Value
                });

            return presentations;
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
    }

    public void UpdatePresentation(Presentation presentation)
    {
        try
        {
            var model = Connection.Presentations.First(p => p.Id == presentation.Id);
            model.Start = presentation.Start;
            model.End = presentation.End;
            Connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
    }

    public Presentation GetPresentation(Presentation presentation)
    {
        try
        {
            var model = Connection.Presentations.First(p => p.Id == presentation.Id);
            return ConvertModelToPresentation(model);
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
    }

    public User GetUser(int userId)
    {
        try
        {
            var model = Connection.Users.First(u => u.Id == userId);
            return new User()
            {
                Id = model.Id,
                Name = model.Name,
                Password = model.Password,
                Username = model.Username,
                CompanyLevel = model.CompanyLevel,
                CreatedAt = model.CreatedAt,
                LastName = model.Lastname,
                SkillLevel = model.SkillLevel,
            };
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
    }

    public Presentation ConvertModelToPresentation(Model.Presentation model)
    {
        return new Presentation
        {
            Id = model.Id,
            End = model.End,
            Start = model.Start,
            UserId = model.UserId.Value
        };
    }
}