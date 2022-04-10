using GrpcService1.Domain.Entities;

namespace GrpcService1.App.Core.OffTime;

public interface IDatabase
{
    public void RecordOffTime(Domain.Entities.OffTime offTime);
    public Domain.Entities.OffTime[]  GetOffTimeHistory(User user,DateTime from, DateTime to);
}