#region

using GrpcService1.Domain.Entities;
using GrpcService1.Domain.Errors;

#endregion

namespace GrpcService1.App.Core.OffTime;

public class Core
{
    /*
     * These are the constants that will be used to demonstrate the off time status.
     */
    public string ApprovedOffTimeCode;

    private readonly IDatabase Database;
    private readonly InternalError InternalError;

    /*
     * Number of seconds that if exceeded, the manager must approve the
     * off time.
     */
    private readonly int OffTimeRestriction;
    private readonly OffTimeRestrictionExceeded OffTimeRestrictionExceeded;
    private readonly OperationSuccessful OperationSuccessful;
    public string RejectedOffTimeCode;
    public string WaitingOffTimeCode;

    public Core(OffTimeDependencies dependencies, OffTimeCoreConfigs configs)
    {
        InternalError = new InternalError(configs.InternalErrorMessage);
        OperationSuccessful = new OperationSuccessful(configs.OperationsuccessfulMessage);
        OffTimeRestrictionExceeded = new OffTimeRestrictionExceeded(configs.OffTimeRestrictionExceededMessage);
        OffTimeRestriction = configs.OffTimeRestriction;

        Database = dependencies.Database;
    }

    /*
     * @TODO Add extra checking for off time availability 
     */
    public Domain.Errors.Status RecordOffTime(User user, Domain.Entities.OffTime offTime)
    {
        long totalOffTimeDuration = 0;
        var history = Database.GetOffTimeHistory(user, DateTime.Now.AddMonths(-1), DateTime.Now);
        foreach (var offTimeHistory in history)
            totalOffTimeDuration += offTimeHistory.to.ToTimestamp().Seconds - offTime.@from.ToTimestamp().Seconds;

        switch (totalOffTimeDuration >= OffTimeRestriction)
        {
            case true:
                return OffTimeRestrictionExceeded;
        }

        try
        {
            offTime.Status = WaitingOffTimeCode;
            Database.RecordOffTime(offTime);
        }
        catch (Exception e)
        {
            return InternalError;
        }

        return OperationSuccessful;
    }

    public Domain.Errors.Status ApproveOffTime(Domain.Entities.OffTime offTime)
    {
        try
        {
            Database.ChangeOffTimeStatus(offTime, ApprovedOffTimeCode);
        }
        catch (Exception e)
        {
            return InternalError;
        }

        return OperationSuccessful;
    }

    public Domain.Errors.Status RejectOffTime(Domain.Entities.OffTime offTime)
    {
        try
        {
            Database.ChangeOffTimeStatus(offTime, RejectedOffTimeCode);
        }
        catch (Exception e)
        {
            return InternalError;
        }

        return OperationSuccessful;
    }

    public class OffTimeCoreConfigs
    {
        public string ApprovedOffTimeCode;
        public string InternalErrorMessage;
        public int OffTimeRestriction;
        public string OffTimeRestrictionExceededMessage;
        public string OperationsuccessfulMessage;
        public string RejectedOffTimeCode;
        public string WaitingOffTimeCode;
    }

    public class OffTimeDependencies
    {
        public IDatabase Database;
    }
}