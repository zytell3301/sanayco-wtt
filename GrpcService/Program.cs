#region

using ErrorReporter;
using GrpcService1.App.Core.Tasks;
using GrpcService1.App.Database.Model;
using GrpcService1.App.Database.OffTime;
using GrpcService1.App.Database.Presentations;
using GrpcService1.App.Database.Projects;
using GrpcService1.App.Database.Tasks;
using GrpcService1.App.Handlers.Http;
using GrpcService1.App.Handlers.Http.tasks;
using GrpcService1.App.TokenSource;
using GrpcService1.Domain.Errors;
using Microsoft.EntityFrameworkCore;

#endregion

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
builder.Services.AddControllers();

// Database connection string
var connectionString = "Server=localhost,50296;Database=wtt;Trusted_Connection=True;MultipleActiveResultSets=true;";

// Error reporter instance tha will be passed to all classes for reporting errors
IErrorReporter reporter = new FakeReporter();

// Database connection and database instances for core classes
var connection = new wttContext(new DbContextOptions<wttContext>());
IDatabase tasksDB = new Tasks(connection, reporter);
GrpcService1.App.Core.Presentation.IDatabase presentationDB = new Presentations(connection, reporter);
GrpcService1.App.Core.OffTime.IDatabase offTimesDB = new OffTimes(connection, reporter);
GrpcService1.App.Core.Projects.IDatabase projectsDB = new Projects(connection, reporter);

// Adding core classes to container
builder.Services.AddSingleton(new Core(new Core.TasksCoreDependencies
{
    Database = tasksDB
}, new Core.TasksCoreConfigs
{
    OperationSuccessfulMessage = "OperationSuccessfulMessage",
    InternalErrorMessage = "InternalErrorMessage",

    ApprovedTaskCode = "ApprovedTaskCode",
    UnApprovedTaskCode = "UnApprovedTaskCode",
    WaitingTaskCode = "WaitingTaskCode"
}));
builder.Services.AddSingleton(new GrpcService1.App.Core.Presentation.Core(
    new GrpcService1.App.Core.Presentation.Core.PresentationCoreDependencies
    {
        Database = presentationDB
    }, new GrpcService1.App.Core.Presentation.Core.PresentationCoreConfigs
    {
        OperationSuccessfulMessage = "OperationSuccessfulMessage",
        InternalErrorMessage = "InternalErrorMessage"
    }));
builder.Services.AddSingleton(new GrpcService1.App.Core.OffTime.Core(
    new GrpcService1.App.Core.OffTime.Core.OffTimeDependencies
    {
        Database = offTimesDB
    }, new GrpcService1.App.Core.OffTime.Core.OffTimeCoreConfigs
    {
        OperationSuccessfulMessage = "OperationSuccessfulMessage",
        InternalErrorMessage = "InternalErrorMessage",
        OffTimeRestriction = 1000000000,
        ApprovedOffTimeCode = "ApprovedOffTimeCode",
        RejectedOffTimeCode = "RejectedOffTimeCode",
        WaitingOffTimeCode = "WaitingOffTimeCode",
        OffTimeRestrictionExceededMessage = "OffTimeRestrictionExceededMessage"
    }));
builder.Services.AddSingleton(new GrpcService1.App.Core.Projects.Core(
    new GrpcService1.App.Core.Projects.Core.ProjectsCoreDependencies
    {
        Database = projectsDB
    }, new GrpcService1.App.Core.Projects.Core.ProjectsCoreConfigs
    {
        InternalErrorMessage = "InternalErrorMessage",
        OperationSuccessfulMessage = "OperationSuccessfulMessage",
        CreatorProjectMemberCode = "CreatorProjectMemberCode"
    }));

builder.Services.AddSingleton<ITokenSource>(new TokenSource(connection));
builder.Services.AddSingleton(new AuthenticationFailed("Authentication failed"));

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200", "https://localhost:4200"));


// Configure the HTTP request pipeline.
// app.MapGet("/",
// () =>
// "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
app.MapControllers();

app.Run();