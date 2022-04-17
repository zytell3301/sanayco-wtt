using ErrorReporter;
using GrpcService1.App.Core.Tasks;
using GrpcService1.App.Database.Presentations;
using GrpcService1.App.Database.Tasks;
using Connection = GrpcService1.App.Database.Tasks.Connection;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
builder.Services.AddControllers();

string connectionString = "Server=localhost,50296;Database=wtt;Trusted_Connection=True;MultipleActiveResultSets=true;";
IErrorReporter reporter = new FakeReporter();
Connection tasksConnection =
    new Connection(connectionString);
GrpcService1.App.Database.Presentations.Connection
    presentationConnection = new GrpcService1.App.Database.Presentations.Connection(connectionString);
IDatabase tasksDB = new Tasks(tasksConnection, reporter);
GrpcService1.App.Core.Presentation.IDatabase presentationDB = new Presentations(presentationConnection, reporter);
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
builder.Services.AddSingleton<GrpcService1.App.Core.Presentation.Core>(new GrpcService1.App.Core.Presentation.Core(
    new GrpcService1.App.Core.Presentation.Core.PresentationCoreDependencies()
    {
        Database = presentationDB,
    }, new GrpcService1.App.Core.Presentation.Core.PresentationCoreConfigs()
    {
        OperationSuccessfulMessage = "OperationSuccessfulMessage",
        InternalErrorMessage = "InternalErrorMessage",
    }));

// Add services to the container.
var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200", "https://localhost:4200"));


// Configure the HTTP request pipeline.
// app.MapGet("/",
// () =>
// "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
app.MapControllers();

app.Run();