using ErrorReporter;
using GrpcService1.App.Core.Tasks;
using GrpcService1.App.Database.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
builder.Services.AddControllers();
Connection connection =
    new Connection("Server=localhost,50296;Database=wtt;Trusted_Connection=True;MultipleActiveResultSets=true;");
IDatabase tasksDB = new Tasks(connection, new Reporter());
builder.Services.AddSingleton<Core>(new Core(new Core.TasksCoreDependencies()
{
    Database = tasksDB,
}, new Core.TasksCoreConfigs()
{
    OperationSuccessfulMessage = "OperationSuccessfulMessage",
    InternalErrorMessage = "InternalErrorMessage",

    ApprovedTaskCode = "ApprovedTaskCode",
    UnApprovedTaskCode = "UnApprovedTaskCode",
    WaitingTaskCode = "WaitingTaskCode",
}));
// Add services to the container.
var app = builder.Build();

// Configure the HTTP request pipeline.
// app.MapGet("/",
// () =>
// "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
app.MapControllers();

app.Run();