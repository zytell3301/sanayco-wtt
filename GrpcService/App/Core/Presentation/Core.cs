namespace GrpcService1.App.Core.Presentation;

public class Core
{
    private IDatabase database;

    public Core(IDatabase database)
    {
        this.database = database;
    }
}