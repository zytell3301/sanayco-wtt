namespace GrpcService1.App.Core.Missions;

public interface IDatabase
{
    public void RecordMission(Domain.Entities.Mission mission);
}