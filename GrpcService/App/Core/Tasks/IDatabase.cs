using Task = GrpcService1.Domain.Entities.Task;

namespace GrpcService1.App.Core.Tasks;

public interface IDatabase
{
    public void RecordTask(Task task);

    public void DeleteTask(Task task);

    public void EditTask(Task task);

    public void ChangeTaskStatus(Task task, string status);
}