namespace GrpcService1.App.Core.OffTime;

public interface IDatabase
{
    public void RecordOffTime(Domain.Entities.OffTime offTime);
}