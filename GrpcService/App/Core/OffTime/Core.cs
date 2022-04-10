using Google.Protobuf.WellKnownTypes;
using GrpcService1.Domain.Entities;
using GrpcService1.Domain.Errors;

namespace GrpcService1.App.Core.OffTime;

public class Core
{
    private InternalError InternalError;
    private OperationSuccessful OperationSuccessful;
    private OffTimeRestrictionExceeded OffTimeRestrictionExceeded;

    /*
     * These are the constants that will be used to demonstrate the off time status.
     */
    public string ApprovedOffTimeCode;
    public string RejectedOffTimeCode;
    public string WaitingOffTimeCode;

    private IDatabase Database;

    /*
     * Number of seconds that if exceeded, the manager must approve the
     * off time.
     */
    private int OffTimeRestriction;

    public class OffTimeCoreConfigs
    {
        public string OperationsuccessfulMessage;
        public string InternalErrorMessage;
        public string OffTimeRestrictionExceededMessage;
        public int OffTimeRestriction;

        public string ApprovedOffTimeCode;
        public string RejectedOffTimeCode;
        public string WaitingOffTimeCode;
    }

    public class OffTimeDependencies
    {
        public IDatabase Database;
    }

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
    public Status RecordOffTime(User user, Domain.Entities.OffTime offTime)
    {
        long totalOffTimeDuration = 0;
        Domain.Entities.OffTime[] history = Database.GetOffTimeHistory(user, DateTime.Now.AddMonths(-1), DateTime.Now);
        foreach (var offTimeHistory in history)
        {
            totalOffTimeDuration += offTimeHistory.to.ToTimestamp().Seconds - offTime.@from.ToTimestamp().Seconds;
        }

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

    public Status ApproveOffTime(Domain.Entities.OffTime offTime)
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

    public Status RejectOffTime(Domain.Entities.OffTime offTime)
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
}