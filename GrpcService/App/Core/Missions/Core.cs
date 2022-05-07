#region

using GrpcService1.Domain.Entities;
using GrpcService1.Domain.Errors;

#endregion

namespace GrpcService1.App.Core.Missions;

public class Core
{
    private readonly IDatabase Database;
    private readonly InternalError InternalError;

    public Core(MissionsCoreDependencies dependencies, MissionsCoreConfigs configs)
    {
        Database = dependencies.Database;
        InternalError = new InternalError(configs.InternalErrorMessage);
    }

    public bool CheckMissionOwnership(int missionId, int userId)
    {
        var mission = Database.GetMission(new Mission()
        {
            Id = missionId,
        });
        return mission.MemberId == userId;
    }

    public void RecordMission(Mission mission)
    {
        try
        {
            mission.IsVerified = null;
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
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public void UpdateMission(Mission mission)
    {
        try
        {
            Database.UpdateMission(mission);
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

    public void ApproveMission(Mission mission)
    {
        try
        {
            mission.IsVerified = true;
            Database.ChangeMissionStatus(mission);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public void RejectMission(Mission mission)
    {
        try
        {
            mission.IsVerified = false;
            Database.ChangeMissionStatus(mission);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public void SetMissionWaiting(Mission mission)
    {
        try
        {
            mission.IsVerified = null;
            Database.ChangeMissionStatus(mission);
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