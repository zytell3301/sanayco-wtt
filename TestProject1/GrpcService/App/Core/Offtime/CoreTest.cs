using GrpcService1.App.Core.OffTime;
using Moq;
using NUnit.Framework;

namespace TestProject1.GrpcService.App.Core.Offtime;

[TestFixture]
public class CoreTest
{
    private GrpcService1.App.Core.OffTime.Core Core;
    private Mock<IDatabase> OffTimesDB;

    [SetUp]
    public void Setup()
    {
        OffTimesDB = new Mock<IDatabase>(MockBehavior.Strict);
        Core = new GrpcService1.App.Core.OffTime.Core(new GrpcService1.App.Core.OffTime.Core.OffTimeDependencies()
        {
            Database = OffTimesDB.Object,
        }, new GrpcService1.App.Core.OffTime.Core.OffTimeCoreConfigs()
        {
            InternalErrorMessage = "InternalErrorMessage",
            OffTimeRestriction = 10,
            OperationSuccessfulMessage = "OperationSuccessfulMessage",
            ApprovedOffTimeCode = "ApprovedOffTimeCode",
            RejectedOffTimeCode = "RejectedOffTimeCode",
            WaitingOffTimeCode = "WaitingOffTimeCode",
            OffTimeRestrictionExceededMessage = "OffTimeRestrictionExceededMessage",
        });
    }
}