#region

using ErrorReporter;
using GrpcService1.App.Core.Missions;
using GrpcService1.App.Database.Model;
using GrpcService1.Domain.Errors;
using Mission = GrpcService1.Domain.Entities.Mission;

#endregion

namespace GrpcService1.App.Database.Missions;

public class Database : IDatabase
{
    private readonly wttContext Connection;
    private readonly IErrorReporter ErrorReporter;
    private readonly InternalError InternalError;

    public Database(MissionsDatabaseDependencies dependencies, MissionsDatabaseConfigs configs)
    {
        Connection = dependencies.Connection;
        ErrorReporter = dependencies.ErrorReporter;

        InternalError = new InternalError(configs.InternalErrorMessage);
    }

    public void RecordMission(Mission mission)
    {
        try
        {
            Connection.Missions.Add(new Model.Mission
            {
                Description = mission.Description,
                MemberId = mission.MemberId,
                ProjectId = mission.ProjectId,
                FromDate = mission.FromDate,
                ToDate = mission.ToDate,
                Location = mission.Location,
                Title = mission.Title,
                IsVerified = mission.IsVerified
            });
            Connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
        }
    }

    public void DeleteMission(Mission mission)
    {
        try
        {
            Connection.Remove(Connection.Missions.First(m => m.Id == mission.Id));
            Connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
        }
    }

    public void UpdateMission(Mission mission)
    {
        try
        {
            var model = Connection.Missions.First(m => m.Id == mission.Id);
            model.Description = mission.Description;
            model.Location = mission.Location;
            model.Title = mission.Title;
            model.FromDate = mission.FromDate;
            model.ToDate = mission.ToDate;
            model.ProjectId = mission.ProjectId;
            Connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
        }
    }

    public Mission GetMission(Mission mission)
    {
        try
        {
            return ConvertModelToMissionEntity(Connection.Missions.First(m => m.Id == mission.Id));
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public void ChangeMissionStatus(Mission mission)
    {
        try
        {
            var model = Connection.Missions.First(m => m.Id == mission.Id);
            model.IsVerified = mission.IsVerified;
            Connection.SaveChanges();
        }
        catch (Exception e)
        {
            ErrorReporter.ReportException(e);
            throw InternalError;
        }
    }

    private Mission ConvertModelToMissionEntity(Model.Mission mission)
    {
        return new Mission
        {
            Id = mission.Id,
            Description = mission.Description,
            FromDate = mission.FromDate,
            MemberId = mission.MemberId,
            ProjectId = mission.ProjectId,
            ToDate = mission.ToDate,
            Location = mission.Location,
            Title = mission.Title,
            IsVerified = mission.IsVerified
        };
    }

    public class MissionsDatabaseDependencies
    {
        public wttContext Connection;
        public IErrorReporter ErrorReporter;
    }

    public class MissionsDatabaseConfigs
    {
        public string InternalErrorMessage;
    }
}