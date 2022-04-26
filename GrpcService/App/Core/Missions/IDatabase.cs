namespace GrpcService1.App.Core.Missions;

public interface IDatabase
{
    public void RecordMission(Domain.Entities.Mission mission);
    public void DeleteMission(Domain.Entities.Mission mission);
    public void UpdateMission(Domain.Entities.Mission mission);
    public Domain.Entities.Mission GetMission(Domain.Entities.Mission mission);
}