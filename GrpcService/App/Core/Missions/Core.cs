#region

using GrpcService1.Domain.Entities;
using GrpcService1.Domain.Errors;

#endregion

namespace GrpcService1.App.Core.Missions;

public class Core
{
    private readonly InternalError InternalError;
    private readonly IDatabase Database;

    public Core(MissionsCoreDependencies dependencies, MissionsCoreConfigs configs)
    {
        Database = dependencies.Database;
        InternalError = new InternalError(configs.InternalErrorMessage);
    }

    public void RecordMission(Mission mission)
    {
        try
        {
            Database.RecordMission(mission);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public void DeleteMission(Mission mission)
    {
        try
        {
            Database.DeleteMission(mission);
        }
        catch (Exception e)
        {
            throw InternalError;
        }
    }

    public void UpdateMission(Mission mission)
    {
        try
        {
            Database.DeleteMission(mission);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public Mission GetMission(Mission mission)
    {
        try
        {
            return Database.GetMission(mission);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public class MissionsCoreDependencies
    {
        public IDatabase Database;
    }

    public class MissionsCoreConfigs
    {
        public string InternalErrorMessage;
    }
}