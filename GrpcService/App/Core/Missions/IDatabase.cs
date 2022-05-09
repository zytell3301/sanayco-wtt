#region

using GrpcService1.Domain.Entities;

#endregion

namespace GrpcService1.App.Core.Missions;

public interface IDatabase
{
    public void RecordMission(Mission mission);
    public void DeleteMission(Mission mission);
    public void UpdateMission(Mission mission);
    public Mission GetMission(Mission mission);
    public void ChangeMissionStatus(Mission mission);
    public List<Domain.Entities.Mission> GetMissionRange(DateTime fromDate, DateTime toDate, int userId,int projectId);
}