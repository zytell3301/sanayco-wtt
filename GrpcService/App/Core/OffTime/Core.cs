#region

using Google.Protobuf.WellKnownTypes;
using GrpcService1.Domain.Entities;
using GrpcService1.Domain.Errors;

#endregion

namespace GrpcService1.App.Core.OffTime;

public class Core
{
    /*
     * These are the constants that will be used to demonstrate the off time status.
     */
    private readonly string ApprovedOffTimeCode;

    private readonly IDatabase Database;
    private readonly InternalError InternalError;

    /*
     * Number of seconds that if exceeded, the manager must approve the
     * off time.
     */
    private readonly int OffTimeRestriction;
    private readonly OffTimeRestrictionExceeded OffTimeRestrictionExceeded;
    private readonly OperationSuccessful OperationSuccessful;
    private readonly string RejectedOffTimeCode;
    private readonly string WaitingOffTimeCode;

    public Core(OffTimeDependencies dependencies, OffTimeCoreConfigs configs)
    {
        InternalError = new InternalError(configs.InternalErrorMessage);
        OperationSuccessful = new OperationSuccessful(configs.OperationSuccessfulMessage);
        OffTimeRestrictionExceeded = new OffTimeRestrictionExceeded(configs.OffTimeRestrictionExceededMessage);
        OffTimeRestriction = configs.OffTimeRestriction;

        ApprovedOffTimeCode = configs.ApprovedOffTimeCode;
        RejectedOffTimeCode = configs.RejectedOffTimeCode;
        WaitingOffTimeCode = configs.WaitingOffTimeCode;

        Database = dependencies.Database;
    }

    public bool CheckOffTimeOwnership(int offTimeId, int userId)
    {
        var offTime = Database.GetOffTime(new Domain.Entities.OffTime
        {
            Id = offTimeId
        });
        return offTime.UserId == userId;
    }

    /*
     * @TODO Add extra checking for off time availability 
     */
    public Domain.Errors.Status RecordOffTime(User user, Domain.Entities.OffTime offTime)
    {
        long totalOffTimeDuration = 0;
        var history = Database.GetOffTimeHistory(user, DateTime.Now.AddMonths(-1), DateTime.Now);
        foreach (var offTimeHistory in history)
            totalOffTimeDuration +=
                offTimeHistory.ToDate.ToTimestamp().Seconds - offTime.FromDate.ToTimestamp().Seconds;

        switch (totalOffTimeDuration >= OffTimeRestriction)
        {
            case true:
                return OffTimeRestrictionExceeded;
        }

        try
        {
            offTime.Status = WaitingOffTimeCode;
            offTime.CreatedAt = DateTime.Now;
            Database.RecordOffTime(offTime);
        }
        catch (Exception)
        {
            return InternalError;
        }

        return OperationSuccessful;
    }

    public void ApproveOffTime(Domain.Entities.OffTime offTime)
    {
        try
        {
            Database.ChangeOffTimeStatus(offTime, ApprovedOffTimeCode);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public void RejectOffTime(Domain.Entities.OffTime offTime)
    {
        try
        {
            Database.ChangeOffTimeStatus(offTime, RejectedOffTimeCode);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public void SetOffTimeStatusWaiting(Domain.Entities.OffTime offTime)
    {
        try
        {
            Database.ChangeOffTimeStatus(offTime, WaitingOffTimeCode);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public void CancelOffTime(Domain.Entities.OffTime offTime)
    {
        try
        {
            Database.DeleteOffTime(offTime);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public Domain.Entities.OffTime GetOffTime(Domain.Entities.OffTime offTime)
    {
        try
        {
            return Database.GetOffTime(offTime);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public void EditOffTime(Domain.Entities.OffTime offTime)
    {
        try
        {
            Database.EditOffTime(offTime);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public class OffTimeCoreConfigs
    {
        public string ApprovedOffTimeCode;
        public string InternalErrorMessage;
        public int OffTimeRestriction;
        public string OffTimeRestrictionExceededMessage;
        public string OperationSuccessfulMessage;
        public string RejectedOffTimeCode;
        public string WaitingOffTimeCode;
    }

    public class OffTimeDependencies
    {
        public IDatabase Database;
    }
}