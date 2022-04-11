#region

using GrpcService1.Domain.Entities;

#endregion

namespace GrpcService1.App.Core.OffTime;

public interface IDatabase
{
    public void RecordOffTime(Domain.Entities.OffTime offTime);
    public Domain.Entities.OffTime[] GetOffTimeHistory(User user, DateTime from, DateTime to);
    public void ChangeOffTimeStatus(Domain.Entities.OffTime offTime, string status);
}