﻿#region

using GrpcService1.Domain.Errors;

#endregion

namespace GrpcService1.App.Core.Tasks;

public class Core
{
    public string ApprovedTaskCode;
    private readonly IDatabase Database;
    private readonly InternalError InternalError;
    private readonly OperationSuccessful OperationSuccessful;
    public string RejectedTaskCode;
    public string WaitingTaskCode;

    public Core(TasksCoreDependencies dependencies, TasksCoreConfigs configs)
    {
        OperationSuccessful = new OperationSuccessful(configs.OperationSuccessfulMessage);
        InternalError = new InternalError(configs.InternalErrorMessage);
        ApprovedTaskCode = configs.ApprovedTaskCode;
        WaitingTaskCode = configs.WaitingTaskCode;
        RejectedTaskCode = configs.UnApprovedTaskCode;

        Database = dependencies.Database;
    }

    /*
     * This method records the given task. 
     */
    public void RecordTask(Domain.Entities.Task task)
    {
        try
        {
            task.Status = WaitingTaskCode;
            Database.RecordTask(task);
        }
        catch (Exception e)
        {
            throw InternalError;
        }
    }

    public Domain.Errors.Status DeleteTask(Domain.Entities.Task task)
    {
        try
        {
            Database.DeleteTask(task);
        }
        catch (Exception e)
        {
            return InternalError;
        }

        return OperationSuccessful;
    }

    public Domain.Errors.Status EditTask(Domain.Entities.Task task)
    {
        try
        {
            Database.EditTask(task);
        }
        catch (Exception e)
        {
            return InternalError;
        }

        return OperationSuccessful;
    }

    public Domain.Errors.Status ApproveTask(Domain.Entities.Task task)
    {
        try
        {
            Database.ChangeTaskStatus(task, ApprovedTaskCode);
        }
        catch (Exception e)
        {
            return InternalError;
        }

        return OperationSuccessful;
    }

    public Domain.Errors.Status RejectTask(Domain.Entities.Task task)
    {
        try
        {
            Database.ChangeTaskStatus(task, RejectedTaskCode);
        }
        catch (Exception e)
        {
            return InternalError;
        }

        return OperationSuccessful;
    }

    public Domain.Entities.Task GetTask(Domain.Entities.Task task)
    {
        try
        {
            return Database.GetTask(task.Id);
        }
        catch (Exception e)
        {
            throw InternalError;
        }
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
    }
}