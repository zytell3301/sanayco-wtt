using GrpcService1.Domain.Entities;
using GrpcService1.Domain.Errors;

namespace GrpcService1.App.Core.Presentation;

public class Core
{
    private IDatabase database;
    private InternalError InternalError;

    public class PresentationCoreConfig
    {
        public string InternalErrorMessage;
    }

    public Core(IDatabase database, PresentationCoreConfig config)
    {
        this.database = database;
        /*
         * Initiate the error instance once and use it forever.
         */
        InternalError = new InternalError(config.InternalErrorMessage);
    }

    public Status RecordPresentation(User user)
    {
        try
        {
            database.RecordPresentation(user);
        }
        catch (InternalError e)
        {
            return new InternalError("");
        }
    }
}