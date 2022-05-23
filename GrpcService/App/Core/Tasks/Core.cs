#region

using GrpcService1.App.Excel;
using GrpcService1.App.Pdf;
using GrpcService1.Domain.Errors;

#endregion

namespace GrpcService1.App.Core.Tasks;

public class Core
{
    private readonly IDatabase Database;
    private readonly InternalError InternalError;
    private readonly OperationSuccessful OperationSuccessful;

    public string ApprovedTaskCode;
    private readonly IExcel Excel;
    private readonly IPdf Pdf;
    public string RejectedTaskCode;
    public string WaitingTaskCode;


    public Core(TasksCoreDependencies dependencies, TasksCoreConfigs configs)
    {
        OperationSuccessful = new OperationSuccessful(configs.OperationSuccessfulMessage);
        InternalError = new InternalError(configs.InternalErrorMessage);
        ApprovedTaskCode = configs.ApprovedTaskCode;
        WaitingTaskCode = configs.WaitingTaskCode;
        RejectedTaskCode = configs.UnApprovedTaskCode;
        Excel = dependencies.Excel;

        Database = dependencies.Database;
        Pdf = dependencies.Pdf;
    }

    public bool CheckTaskOwnership(int taskId, int userId)
    {
        var task = Database.GetTask(taskId);
        return task.UserId == userId;
    }

    /*
     * This method records the given task. 
     */
    public void RecordTask(Domain.Entities.Task task)
    {
        try
        {
            task.Status = WaitingTaskCode;
            task.CreatedAt = DateTime.Now;
            Database.RecordTask(task);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public void DeleteTask(Domain.Entities.Task task)
    {
        try
        {
            Database.DeleteTask(task);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public void EditTask(Domain.Entities.Task task)
    {
        try
        {
            Database.EditTask(task);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public void ApproveTask(Domain.Entities.Task task)
    {
        try
        {
            Database.ChangeTaskStatus(task, ApprovedTaskCode);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public void RejectTask(Domain.Entities.Task task)
    {
        try
        {
            Database.ChangeTaskStatus(task, RejectedTaskCode);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public void SetTaskWaiting(Domain.Entities.Task task)
    {
        try
        {
            Database.ChangeTaskStatus(task, WaitingTaskCode);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public Domain.Entities.Task GetTask(Domain.Entities.Task task)
    {
        try
        {
            return Database.GetTask(task.Id);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public List<Domain.Entities.Task> GetTaskRange(DateTime fromDate, DateTime toDate, int userId, int projectId,
        string workLocation)
    {
        try
        {
            return Database.GetTaskRange(fromDate, toDate, userId, projectId, workLocation);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public List<Domain.Entities.Task> GetUserTasks(DateTime fromDate, DateTime toDate, int userId)
    {
        try
        {
            return Database.GetUserTasks(fromDate, toDate, userId);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public IExcel.IExcelFile GetExcelReport(DateTime fromDate, DateTime toDate, int userId)
    {
        try
        {
            var tasks = Database.GetUserTasks(fromDate, toDate, userId);
            var excel = Excel.NewExcel();
            excel.SetCell(1, 1, "id");
            excel.SetCell(1, 2, "user id");
            excel.SetCell(1, 3, "project id");
            excel.SetCell(1, 4, "description");
            excel.SetCell(1, 5, "status");
            excel.SetCell(1, 6, "title");
            excel.SetCell(1, 7, "start time");
            excel.SetCell(1, 8, "points");
            excel.SetCell(1, 9, "created at");
            excel.SetCell(1, 10, "work location");
            var i = 2;
            foreach (var task in tasks)
            {
                excel.SetCell(i, 1, task.Id.ToString());
                excel.SetCell(i, 2, task.UserId.ToString());
                excel.SetCell(i, 3, task.ProjectId.ToString());
                excel.SetCell(i, 4, task.Description);
                excel.SetCell(i, 5, task.Status);
                excel.SetCell(i, 6, task.Title);
                excel.SetCell(i, 7, task.StartTime.ToString());
                excel.SetCell(i, 8, task.Points.ToString());
                excel.SetCell(i, 9, task.CreatedAt.ToString());
                excel.SetCell(i, 10, task.WorkLocation);
            }

            return excel.GetExcelFile();
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public IPdfBase.IPdfFile GeneratePdf(DateTime fromDate, DateTime toDate, int userId)
    {
        // try
        // {
        var presentations = Database.GetUserTasks(fromDate, toDate, userId);
        var user = Database.GetUser(userId);
        presentations[0].Description = "این یک پیام تست فارسی است";
        return Pdf.NewPdfFile(presentations, new IPdf.ReportInfo
        {
            FromDate = fromDate.ToString(),
            GeneratedAt = DateTime.Now.ToString(),
            ToDate = toDate.ToString(),
            Lastname = user.LastName,
            Name = user.Name
        });
        // }
        // catch (Exception)
        // {
        // throw InternalError;
        // }
    }

    public class TasksCoreConfigs
    {
        public string ApprovedTaskCode;
        public string InternalErrorMessage;
        public string OperationSuccessfulMessage;
        public string UnApprovedTaskCode;
        public string WaitingTaskCode;
    }

    public class TasksCoreDependencies
    {
        public IDatabase Database;
        public IExcel Excel;
        public IPdf Pdf;
    }
}