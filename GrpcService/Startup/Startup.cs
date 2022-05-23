#region

using ErrorReporter;
using GrpcService1.App.Auth;
using GrpcService1.App.Core.Foods;
using GrpcService1.App.Database.Foods;
using GrpcService1.App.Database.Missions;
using GrpcService1.App.Database.Model;
using GrpcService1.App.Database.OffTime;
using GrpcService1.App.Database.Presentations;
using GrpcService1.App.Database.Projects;
using GrpcService1.App.Database.Tasks;
using GrpcService1.App.Database.Users;
using GrpcService1.App.Excel;
using GrpcService1.App.Handlers.Http;
using GrpcService1.App.HashGenerator;
using GrpcService1.App.Pdf.Tasks;
using GrpcService1.App.PermissionsSource;
using GrpcService1.App.ProfilePicturesStorage;
using GrpcService1.App.TokenGenerator;
using GrpcService1.App.TokenSource;
using GrpcService1.Domain.Errors;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Core = GrpcService1.App.Core.Tasks.Core;

#endregion

namespace GrpcService1.Startup;

public class Startup
{
    private readonly AppConfigs Configs = new();

    private readonly ServiceDependencies Dependencies = new();
    private WebApplication App;
    private readonly WebApplicationBuilder Builder;

    public Startup(string[] args)
    {
        // Initialize builder
        Builder = WebApplication.CreateBuilder(args);

        // Add controllers
        AddControllers();

        // Initiating error reporter class to be passed to other dependencies.
        // This method must be called before starting other dependencies or otherwise
        // you will get a null exception error.
        InitiateErrorReporter();

        // Initiate database connection and core databases.
        InitiateDatabaseDependencies();

        // Initiate Auth class that is being injected for auth purposes.
        InitiateAuth();

        // Injects initiated dependencies and cores to service container.
        InjectDependenciesToServiceContainer();

        // Builds the Builder
        BuildApplicationBuilder();

        // Adds required app service and configs.
        AddAppServices();

        // Runs the application
        RunApp();
    }

    private void AddControllers()
    {
        Builder.Services.AddControllers();
    }

    private void InitiateErrorReporter()
    {
        // Fake error reporter is being used for testing and debugging purposes.
        // Change this when moving to production stage.
        Dependencies.ErrorReporter = new FakeReporter();
    }

    private void InitiateDatabaseDependencies()
    {
        Dependencies.DBDependencies.Connection = new wttContext(new DbContextOptions<wttContext>());

        Dependencies.DBDependencies.TasksDB =
            new Tasks(new Tasks.TasksDatabaseDependencies
            {
                Connection = Dependencies.DBDependencies.Connection,
                ErrorReporter = Dependencies.ErrorReporter,
                InternalError = new InternalError("InternalError")
            });
        Dependencies.DBDependencies.PresentationDB =
            new Presentations(new Presentations.PresentationsDatabaseDependencies
            {
                Connection = Dependencies.DBDependencies.Connection,
                ErrorReporter = Dependencies.ErrorReporter,
                InternalError = new InternalError("InternalError")
            });
        Dependencies.DBDependencies.OffTimesDB =
            new OffTimes(new OffTimes.OffTimesDatabaseDependencies
            {
                Connection = Dependencies.DBDependencies.Connection,
                ErrorReporter = Dependencies.ErrorReporter,
                InternalError = new InternalError("InternalError")
            });
        Dependencies.DBDependencies.ProjectsDB =
            new Projects(new Projects.ProjectsDatabaseDependencies
            {
                Connection = Dependencies.DBDependencies.Connection,
                ErrorReporter = Dependencies.ErrorReporter,
                InternalError = new InternalError("InternalError")
            });
        Dependencies.DBDependencies.UsersDB = new Users(new Users.UsersDatabaseDependencies
        {
            Connection = Dependencies.DBDependencies.Connection,
            ErrorReporter = Dependencies.ErrorReporter,
            InternalError = new InternalError("internal error")
        });
        Dependencies.DBDependencies.FoodsDB = new Foods(new Foods.FoodsDatabaseDependencies
        {
            Connection = Dependencies.DBDependencies.Connection,
            ErrorReporter = Dependencies.ErrorReporter,
            InternalError = new InternalError("internalError")
        });
        Dependencies.DBDependencies.MissionsDB = new Database(new Database.MissionsDatabaseDependencies
        {
            Connection = Dependencies.DBDependencies.Connection,
            ErrorReporter = Dependencies.ErrorReporter,
            InternalError = new InternalError("InternalError")
        });
    }

    private void InitiateAuth()
    {
        Dependencies.Auth = new Auth(new Auth.AuthConfigs
        {
            Audience = Configs.AuthConfigs.Audience,
            Issuer = Configs.AuthConfigs.Issuer,
            Key = Configs.AuthConfigs.key,
            SecurityAlgorithm = Configs.AuthConfigs.SecurityAlgorithm
        }, new Auth.AuthDependencies
        {
            ErrorReporter = Dependencies.ErrorReporter,
            InternalError = new InternalError("InternalError")
        });
    }

    private void InjectDependenciesToServiceContainer()
    {
        Builder.Services.AddSingleton(new Core(new Core.TasksCoreDependencies
            {
                Database = Dependencies.DBDependencies.TasksDB,
                Excel = new Excel(),
                Pdf = new Pdf()
            }, new Core.TasksCoreConfigs
            {
                OperationSuccessfulMessage = "OperationSuccessfulMessage",
                InternalErrorMessage = "InternalErrorMessage",

                ApprovedTaskCode = "ApprovedTaskCode",
                UnApprovedTaskCode = "UnApprovedTaskCode",
                WaitingTaskCode = "WaitingTaskCode"
            })
        );
        Builder.Services.AddSingleton(new App.Core.Presentation.Core(
            new App.Core.Presentation.Core.PresentationCoreDependencies
            {
                Database = Dependencies.DBDependencies.PresentationDB,
                Excel = new Excel(),
                Pdf = new App.Pdf.Presentations.Pdf()
            }, new App.Core.Presentation.Core.PresentationCoreConfigs
            {
                OperationSuccessfulMessage = "OperationSuccessfulMessage",
                InternalErrorMessage = "InternalErrorMessage"
            })
        );
        Builder.Services.AddSingleton(new App.Core.OffTime.Core(
            new App.Core.OffTime.Core.OffTimeDependencies
            {
                Database = Dependencies.DBDependencies.OffTimesDB,
                Excel = new Excel()
            }, new App.Core.OffTime.Core.OffTimeCoreConfigs
            {
                OperationSuccessfulMessage = "OperationSuccessfulMessage",
                InternalErrorMessage = "InternalErrorMessage",
                OffTimeRestriction = 1000000000,
                ApprovedOffTimeCode = "ApprovedOffTimeCode",
                RejectedOffTimeCode = "RejectedOffTimeCode",
                WaitingOffTimeCode = "WaitingOffTimeCode",
                OffTimeRestrictionExceededMessage = "OffTimeRestrictionExceededMessage"
            })
        );
        Builder.Services.AddSingleton(new App.Core.Projects.Core(
            new App.Core.Projects.Core.ProjectsCoreDependencies
            {
                Database = Dependencies.DBDependencies.ProjectsDB
            }, new App.Core.Projects.Core.ProjectsCoreConfigs
            {
                InternalErrorMessage = "InternalErrorMessage",
                OperationSuccessfulMessage = "OperationSuccessfulMessage",
                CreatorProjectMemberCode = "CreatorProjectMemberCode"
            })
        );
        Builder.Services.AddSingleton(new App.Core.Foods.Core(
            new App.Core.Foods.Core.FoodsCoreDependencies
            {
                Database = Dependencies.DBDependencies.FoodsDB
            }, new App.Core.Foods.Core.FoodsCoreConfigs
            {
                InternalErrorMessage = "InternalErrorMessage"
            })
        );
        Builder.Services.AddSingleton<ITokenSource>(new TokenSource(Dependencies.DBDependencies.Connection));
        Builder.Services.AddSingleton(new AuthenticationFailed("Authentication failed"));
        Builder.Services.AddSingleton(new BaseHandlerDependencies
        {
            AuthenticationFailed = new AuthenticationFailed("Authentication failed"),
            AuthorizationFailed = new AuthorizationFailed("Authorization failed"),
            PermissionsSource = new PermissionsSource(Dependencies.DBDependencies.Connection),
            TokenSource = new TokenSource(Dependencies.DBDependencies.Connection),
            Auth = Dependencies.Auth
        });
        Builder.Services.AddSingleton(new App.Core.Users.Core(
            new App.Core.Users.Core.UsersCoreConfigs
            {
                ExpirationWindow = 1000000,
                InternalErrorMessage = "InternalErrorMessage",
                InvalidCredentialsMessage = "InvalidCredentialsMessage"
            },
            new App.Core.Users.Core.UsersCoreDependencies
            {
                TokenGenerator = new TokenGenerator(new TokenGenerator.TokenGeneratorConfigs
                {
                    TokenLength = 32,
                    ValidaCharacters = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"
                }),
                ProfilePicturesStorage = new ProfilePicturesStorage(
                    new ProfilePicturesStorage.ProfilePicturesStorageDependencies
                    {
                        ErrorReporter = Dependencies.ErrorReporter,
                        TokenGenerator = new TokenGenerator(new TokenGenerator.TokenGeneratorConfigs
                        {
                            TokenLength = 32,
                            ValidaCharacters = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"
                        })
                    }, new ProfilePicturesStorage.ProfilePicturesStorageConfigs
                    {
                        InternalErrorMessage = "InternalErrorMessage",
                        PicturesRootDirectory = "./"
                    }),
                PermissionsSource = new PermissionsSource(Dependencies.DBDependencies.Connection),
                Database = Dependencies.DBDependencies.UsersDB,
                Hash = new HashGenerator(new HashGenerator.HashGeneratorConfigs
                {
                    HashCost = 12,
                    InternalErrorMessage = "InternalErrorMessage"
                }, new HashGenerator.HashGeneratorDependencies
                {
                    ErrorReporter = Dependencies.ErrorReporter
                }),
                Auth = Dependencies.Auth
            })
        );
        Builder.Services.AddSingleton(new App.Core.Missions.Core(
            new App.Core.Missions.Core.MissionsCoreDependencies
            {
                Database = Dependencies.DBDependencies.MissionsDB
            },
            new App.Core.Missions.Core.MissionsCoreConfigs
            {
                InternalErrorMessage = "InternalErrorMessage"
            })
        );
    }

    private void BuildApplicationBuilder()
    {
        App = Builder.Build();
    }

    private void AddAppServices()
    {
        App.UseHttpsRedirection();
        App.UseCors(x =>
            x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200", "https://localhost:4200"));
        App.MapControllers();
    }

    private void RunApp()
    {
        App.Run();
    }


    private class ServiceDependencies
    {
        public Auth Auth;
        public readonly DatabaseDependencies DBDependencies = new();
        public IErrorReporter ErrorReporter;

        public class DatabaseDependencies
        {
            public wttContext Connection;
            public IDatabase FoodsDB;
            public App.Core.Missions.IDatabase MissionsDB;
            public App.Core.OffTime.IDatabase OffTimesDB;
            public App.Core.Presentation.IDatabase PresentationDB;
            public App.Core.Projects.IDatabase ProjectsDB;
            public App.Core.Tasks.IDatabase TasksDB;
            public App.Core.Users.IDatabase UsersDB;
        }
    }

    private class AppConfigs
    {
        public const string ConnectionString =
            "Server=localhost,50296;Database=wtt;Trusted_Connection=True;MultipleActiveResultSets=true;";

        public readonly Auth AuthConfigs = new();

        public class Auth
        {
            public readonly string Audience = "https://localhost:5001";
            public readonly string Issuer = "https://localhost:5001";
            public readonly string key = "asdv234234^&%&^%&^hjsdfb2%%%";
            public readonly string SecurityAlgorithm = SecurityAlgorithms.HmacSha512;
        }
    }
}