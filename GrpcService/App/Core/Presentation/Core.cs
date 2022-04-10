using GrpcService1.Domain.Entities;
using GrpcService1.Domain.Errors;

namespace GrpcService1.App.Core.Presentation;

public class Core
{
    private IDatabase database;
    private InternalError InternalError;

    public class PresentationCoreConfigs
    {
        public string InternalErrorMessage;
    }

    public class PresentationCoreDependencies
    {
        public IDatabase Database;
    }

    public Core(PresentationCoreDependencies dependencies, PresentationCoreConfigs configs)
    {
        this.database = dependencies.Database;
        /*
         * Initiate the error instance once and use it forever.
         */
        InternalError = new InternalError(configs.InternalErrorMessage);
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