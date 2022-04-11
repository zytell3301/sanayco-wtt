using GrpcService1.App.Core.Tasks;

namespace GrpcService1.Database.Tasks;

public class Tasks : IDatabase
{
    public void RecordTask(Domain.Entities.Task task)
    {
        throw new NotImplementedException();
    }

    public void DeleteTask(Domain.Entities.Task task)
    {
        throw new NotImplementedException();
    }

    public void EditTask(Domain.Entities.Task task)
    {
        throw new NotImplementedException();
    }

    public void ChangeTaskStatus(Domain.Entities.Task task, string status)
    {
        throw new NotImplementedException();
    }
}