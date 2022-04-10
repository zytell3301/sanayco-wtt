using Task = GrpcService1.Domain.Entities.Task;

namespace GrpcService1.App.Core.Tasks;

public interface IDatabase
{
    public void RecordTask(Task task);

    public void DeleteTask(Task task);
}