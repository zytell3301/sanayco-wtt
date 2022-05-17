#region

using System;
using System.Collections.Generic;
using GrpcService1.App.Core.Tasks;
using GrpcService1.Domain.Entities;
using GrpcService1.Domain.Errors;
using Moq;
using NUnit.Framework;

#endregion

namespace TestProject1.GrpcService.App.Core.Tasks;

[TestFixture]
public class CoreTest
{
    [SetUp]
    public void Setup()
    {
        TasksDB = new Mock<IDatabase>(MockBehavior.Strict);
        Core = new GrpcService1.App.Core.Tasks.Core(new GrpcService1.App.Core.Tasks.Core.TasksCoreDependencies
        {
            Database = TasksDB.Object
        }, new GrpcService1.App.Core.Tasks.Core.TasksCoreConfigs
        {
            ApprovedTaskCode = "ApprovedTaskCode",
            InternalErrorMessage = "InternalErrorMessage",
            OperationSuccessfulMessage = "OperationSuccessfulMessage",
            WaitingTaskCode = "WaitingTaskCode",
            UnApprovedTaskCode = "UnApprovedTaskCode"
        });
    }

    private Mock<IDatabase> TasksDB;
    private GrpcService1.App.Core.Tasks.Core Core;

    private readonly Task DummyTask = new()
    {
        Id = 1,
        UserId = 1,
        Description = "",
        Points = 3,
        Status = "",
        Title = "",
        CreatedAt = DateTime.Now,
        ProjectId = 1,
        WorkLocation = "",
        StartTime = DateTime.Now
    };

    // Normal test case
    [Test]
    public void test_RecordTask()
    {
        TasksDB.Setup(d => d.RecordTask(DummyTask));
        Assert.DoesNotThrow(() => Core.RecordTask(DummyTask));
    }

    // Test case for repository failure
    [Test]
    public void test_RecordTask_1()
    {
        TasksDB.Setup(d => d.RecordTask(DummyTask)).Throws(new InternalError(""));
        Assert.Throws<InternalError>(() => Core.RecordTask(DummyTask));
    }

    // Normal test case
    [Test]
    public void test_CheckTaskOwnership()
    {
        TasksDB.Setup(d => d.GetTask(DummyTask.Id)).Returns(DummyTask);
        Assert.IsTrue(Core.CheckTaskOwnership(DummyTask.Id, DummyTask.UserId));
    }

    // Test case for failure in validating applicant user for requested resource
    [Test]
    public void test_CheckTaskOwnership_1()
    {
        TasksDB.Setup(d => d.GetTask(DummyTask.Id)).Returns(new Task
        {
            UserId = 2 // Something different from DummyTasks userId
        });
        Assert.IsFalse(Core.CheckTaskOwnership(DummyTask.Id, DummyTask.UserId));
    }

    private List<Task> GetTaskRange()
    {
        var range = new List<Task>();
        range.Add(DummyTask);
        range.Add(DummyTask);
        return range;
    }

    private class GetTaskRangeParams
    {
        public DateTime fromDate;
        public int projectId;
        public DateTime toDate;
        public int userId;
        public string workLocation;
    }

    private GetTaskRangeParams GenerateGetTaskRangeParams()
    {
        return new GetTaskRangeParams
        {
            fromDate = DateTime.Now,
            toDate = DateTime.Now.AddSeconds(1),
            projectId = 1,
            userId = 1,
            workLocation = "home"
        };
    }


    // Normal test case 
    [Test]
    public void test_GetTaskRange()
    {
        var parameters = GenerateGetTaskRangeParams();
        TasksDB.Setup(d => d.GetTaskRange(parameters.fromDate, parameters.toDate, parameters.userId,
            parameters.projectId, parameters.workLocation)).Returns(GetTaskRange());
        Assert.AreEqual(GetTaskRange(), Core.GetTaskRange(parameters.fromDate, parameters.toDate, parameters.userId,
            parameters.projectId, parameters.workLocation));
    }

    // Test case for empty task range
    [Test]
    public void test_GetTaskRange_1()
    {
        var parameters = GenerateGetTaskRangeParams();
        TasksDB.Setup(d => d.GetTaskRange(parameters.fromDate, parameters.toDate, parameters.userId,
            parameters.projectId, parameters.workLocation)).Returns(new List<Task>());
        Assert.AreEqual(new List<Task>(), Core.GetTaskRange(parameters.fromDate, parameters.toDate, parameters.userId,
            parameters.projectId, parameters.workLocation));
    }
}