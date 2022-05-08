#region

using GrpcService1.Domain.Entities;
using GrpcService1.Domain.Errors;

#endregion

namespace GrpcService1.App.Core.Presentation;

public class Core
{
    private readonly IDatabase database;
    private readonly InternalError InternalError;
    private readonly OperationSuccessful OperationSuccessful;

    public Core(PresentationCoreDependencies dependencies, PresentationCoreConfigs configs)
    {
        database = dependencies.Database;
        /*
         * Initiate the error instance once and use it forever.
         */
        InternalError = new InternalError(configs.InternalErrorMessage);
        OperationSuccessful = new OperationSuccessful(configs.OperationSuccessfulMessage);
    }

    /*
     * This method records given user's presentation time 
     */
    public void RecordPresentation(User user)
    {
        try
        {
            database.RecordPresentation(user);
        }
        catch (InternalError)
        {
            throw InternalError;
        }
    }

    /*
     * This method records the end of a user's presentation time
     */
    public void RecordPresentationEnd(User user)
    {
        try
        {
            database.RecordPresentationEnd(user);
        }
        catch (InternalError)
        {
            throw InternalError;
        }
    }

    /*
     * This method gets the latest representation time of the supplied user.
     * It is used for calculating users representation time or maybe other
     * feature updates.
     */
    public DateTime GetPresentationTime(User user)
    {
        DateTime presentationTime;
        try
        {
            presentationTime = database.GetPresentationTime(user);
        }
        catch (Exception)
        {
            throw InternalError;
        }

        return presentationTime;
    }

    public List<Domain.Entities.Presentation> GetPresentationsRange(DateTime fromDate, DateTime toDate, int userId)
    {
        try
        {
            return database.GetPresentationsRange(fromDate, toDate, userId);
        }
        catch (Exception)
        {
            throw InternalError;
        }
    }

    public class PresentationCoreConfigs
    {
        public string InternalErrorMessage;
        public string OperationSuccessfulMessage;
    }

    public class PresentationCoreDependencies
    {
        public IDatabase Database;
    }
}