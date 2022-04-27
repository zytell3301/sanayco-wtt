﻿#region

using ErrorReporter;
using GrpcService1.App.Core.Missions;
using GrpcService1.App.Database.Model;
using GrpcService1.Domain.Errors;
using Mission = GrpcService1.Domain.Entities.Mission;

#endregion

namespace GrpcService1.App.Database.Missions;

public class Database : IDatabase
{
    private readonly InternalError InternalError;
    private readonly wttContext Connection;
    private readonly IErrorReporter ErrorReporter;

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
                ToDate = mission.ToDate
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
            model.Description = mission.Description; // Other fields are not included because it is not reasonable
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

    private Mission ConvertModelToMissionEntity(Model.Mission mission)
    {
        return new Mission
        {
            Id = mission.Id,
            Description = mission.Description,
            FromDate = mission.FromDate,
            MemberId = mission.MemberId,
            ProjectId = mission.ProjectId,
            ToDate = mission.ToDate
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