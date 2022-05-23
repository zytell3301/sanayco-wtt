#region

using GrpcService1.Domain.Entities;

#endregion

namespace GrpcService1.App.Core.Tasks;

public interface IDatabase
{
    public void RecordTask(Domain.Entities.Task task);

    public void DeleteTask(Domain.Entities.Task task);

    public void EditTask(Domain.Entities.Task task);

    public void ChangeTaskStatus(Domain.Entities.Task task, string status);

    public Domain.Entities.Task GetTask(int taskId);

    public List<Domain.Entities.Task> GetTaskRange(DateTime fromDate, DateTime toDate, int userId, int projectId,
        string workLocation);

    public List<Domain.Entities.Task> GetUserTasks(DateTime fromDate, DateTime toDate, int userId);

    public User GetUser(int userId);
}