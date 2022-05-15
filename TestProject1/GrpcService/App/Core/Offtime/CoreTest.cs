using System;
using System.Collections.Generic;
using GrpcService1.App.Core.OffTime;
using GrpcService1.Domain.Entities;
using GrpcService1.Domain.Errors;
using Moq;
using NUnit.Framework;

namespace TestProject1.GrpcService.App.Core.Offtime;

[TestFixture]
public class CoreTest
{
    private GrpcService1.App.Core.OffTime.Core Core;
    private Mock<IDatabase> OffTimesDB;

    private int OffTimeRstriction = 10;

    private static GrpcService1.Domain.Entities.OffTime DummyOffTime = new OffTime()
    {
        Id = 1,
        Description = "",
        Status = "",
        CreatedAt = DateTime.Now,
        FromDate = DateTime.Now,
        ToDate = DateTime.Now,
        UserId = 1,
    };

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
            OffTimeRestriction = OffTimeRstriction,
            OperationSuccessfulMessage = "OperationSuccessfulMessage",
            ApprovedOffTimeCode = "ApprovedOffTimeCode",
            RejectedOffTimeCode = "RejectedOffTimeCode",
            WaitingOffTimeCode = "WaitingOffTimeCode",
            OffTimeRestrictionExceededMessage = "OffTimeRestrictionExceededMessage",
        });
    }

    // Normal test case
    [Test]
    public void test_GetOffTime()
    {
        OffTimesDB.Setup(o => o.GetOffTime(DummyOffTime)).Returns(DummyOffTime);
        Assert.AreEqual(DummyOffTime, Core.GetOffTime(DummyOffTime));
    }

    // Test case for repository internal failure
    [Test]
    public void test_GetOffTime_1()
    {
        OffTimesDB.Setup(o => o.GetOffTime(DummyOffTime)).Throws(new InternalError(""));
        Assert.Throws<InternalError>(() => Core.GetOffTime(DummyOffTime));
    }

    private class GetOffTimeRangeParams
    {
        public DateTime fromDate;
        public DateTime toDate;
        public int userId;
    }

    private GetOffTimeRangeParams GenerateGetOffTimeRangeParams()
    {
        return new GetOffTimeRangeParams()
        {
            userId = 1,
            fromDate = DateTime.Now,
            toDate = DateTime.Now.AddSeconds(10),
        };
    }

    public List<OffTime> GetOffTimeRange()
    {
        var offTimes = new List<OffTime>();
        offTimes.Add(DummyOffTime);
        offTimes.Add(DummyOffTime);
        return offTimes;
    }


    // Normal test case
    [Test]
    public void test_GetOffTimeRange()
    {
        var parameters = GenerateGetOffTimeRangeParams();
        OffTimesDB.Setup(o => o.GetOffTimeListRange(parameters.fromDate, parameters.toDate, parameters.userId))
            .Returns(GetOffTimeRange());
        Assert.AreEqual(GetOffTimeRange(),
            Core.GetOffTimeListRange(parameters.fromDate, parameters.toDate, parameters.userId));
    }

    [Test]
    public void test_GetOffTimeRange_1()
    {
        var parameters = GenerateGetOffTimeRangeParams();
        OffTimesDB.Setup(o => o.GetOffTimeListRange(parameters.fromDate, parameters.toDate, parameters.userId))
            .Returns(new List<OffTime>());
        Assert.AreEqual(new List<OffTime>(),
            Core.GetOffTimeListRange(parameters.fromDate, parameters.toDate, parameters.userId));
    }

    [Test]
    public void test_GetOffTimeRange_2()
    {
        var parameters = GenerateGetOffTimeRangeParams();
        OffTimesDB.Setup(o => o.GetOffTimeListRange(parameters.fromDate, parameters.toDate, parameters.userId))
            .Throws(new InternalError(""));
        Assert.Throws<InternalError>(() =>
            Core.GetOffTimeListRange(parameters.fromDate, parameters.toDate, parameters.userId));
    }

    private class RecordOffTimeParams
    {
        public GetOffTimeHistory GetOffTimeHistoryParameters;
        public CoreRecordOffTimeParams CoreRecordOffTimeParameters;

        internal class GetOffTimeHistory
        {
            public GetOffTimeHistoryParams GetOffTimeHistoryParameters;
            public GetOffTimeHistoryResp GetOffTimeHistoryResponse;

            public class GetOffTimeHistoryParams
            {
                public User user;
                public DateTime from;
                public DateTime to;
            }

            public class GetOffTimeHistoryResp
            {
                public List<OffTime> OffTimes;
            }
        }

        internal class CoreRecordOffTimeParams
        {
            public User user;
            public OffTime offTime;
        }

        public static RecordOffTimeParams GenerateParams()
        {
            var offTimes = new List<OffTime>();
            offTimes.Add(DummyOffTime);
            offTimes.Add(DummyOffTime);
            return new RecordOffTimeParams()
            {
                CoreRecordOffTimeParameters = new CoreRecordOffTimeParams()
                {
                    user = new User()
                    {
                        Id = 1,
                    },
                    offTime = new OffTime()
                    {
                        UserId = 1,
                        CreatedAt = DateTime.Now,
                        Status = "WaitingOffTimeCode",
                    }
                },
                GetOffTimeHistoryParameters = new GetOffTimeHistory()
                {
                    GetOffTimeHistoryParameters = new GetOffTimeHistory.GetOffTimeHistoryParams()
                    {
                        @from = DateTime.Now,
                        to = DateTime.Now.AddMonths(-1),
                        user = new User()
                        {
                            Id = 1,
                        }
                    },
                    GetOffTimeHistoryResponse = new GetOffTimeHistory.GetOffTimeHistoryResp()
                    {
                        OffTimes = offTimes,
                    }
                },
            };
        }
    }

    [Test]
    public void test_RecordOffTime()
    {
        var data = RecordOffTimeParams.GenerateParams();
        data.GetOffTimeHistoryParameters.GetOffTimeHistoryResponse.OffTimes.Add(new OffTime()
        {
            FromDate = DateTime.Now,
            ToDate = DateTime.Now.AddYears(OffTimeRstriction +
                                           1) // We make sure that the total off-time will definitely exceed specified range
        });
        OffTimesDB.Setup(o => o.GetOffTimeHistory(It.IsAny<User>(),
            It.IsAny<DateTime>(),
            It.IsAny<DateTime>())).Returns(data.GetOffTimeHistoryParameters.GetOffTimeHistoryResponse.OffTimes);
        Assert.Throws<OffTimeRestrictionExceeded>(() =>
            Core.RecordOffTime(data.CoreRecordOffTimeParameters.user, data.CoreRecordOffTimeParameters.offTime));
    }
}