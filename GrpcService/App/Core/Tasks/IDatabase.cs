namespace GrpcService1.App.Core.Tasks;

public interface IDatabase
{
    public void RecordTask(Domain.Entities.Task task);

    public void DeleteTask(Domain.Entities.Task task);

    public void EditTask(Domain.Entities.Task task);

    public void ChangeTaskStatus(Domain.Entities.Task task, string status);
}