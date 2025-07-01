using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTimeTracker.Application.TaskItems.Commands.CreateTaskItem;
using TaskTimeTracker.Application.TaskSessions.Commands.StartTaskSession;
using TaskTimeTracker.Application.TaskSessions.Commands.StopTaskSession;
using TaskTimeTracker.Application.TaskSessions.Queries.GetTaskSessionDuration;
using TaskTimeTracker.Domain.Exceptions.TaskItem;
using TaskTimeTracker.Domain.Exceptions.TaskSession;
using static TaskTimeTracker.Application.FunctionalTests.Testing;

namespace TaskTimeTracker.Application.FunctionalTests.TaskSessions.Commands;
public class StopTaskSessionTests : BaseTestFixture
{

    [Test]
    public async Task ShouldEndTaskSession()
    {
        var userId = await RunAsAdministratorAsync();

        var createTaskCommand = new CreateTaskItemCommand()
        {
            Name = "Test Task",
            Description = "This is a test task"
        };

        var taskId = await SendAsync(createTaskCommand);

        var startSessionCommand = new StartTaskSessionCommand()
        {
            TaskId = taskId
        };

        var stopSessionCommand = new StopTaskSessionCommand()
        {
            TaskId = taskId
        };


        await SendAsync(startSessionCommand);

        //await Task.Delay(TimeSpan.FromSeconds(30));
        
        var result = await SendAsync(stopSessionCommand);

        result.Should().BeTrue();

        //var getTaskSessionDuration = new GetTaskSessionDurationQuery()
        //{
        //    TaskId = taskId
        //};

        //var duration = await SendAsync(getTaskSessionDuration);
        //duration.Should().Be(TimeSpan.FromSeconds(30).Seconds);
    }

    [Test]
    public async Task ShouldThrowTaskItemNotFoundException()
    {
        var userId = await RunAsAdministratorAsync();

        //var createTaskCommand = new CreateTaskItemCommand()
        //{
        //    Name = "Test Task",
        //    Description = "This is a test task"
        //};

        //var taskId = await SendAsync(createTaskCommand);

        var stopSessionCommand = new StopTaskSessionCommand()
        {
            TaskId = 999
        };

        await FluentActions.Invoking(() =>
            SendAsync(stopSessionCommand)).Should().ThrowAsync<TaskItemNotFoundException>();
    }

    [Test]
    public async Task ShouldThrowNoStartedTaskSessionWasFoundException()
    {
        var userId = await RunAsAdministratorAsync();

        var createTaskCommand = new CreateTaskItemCommand()
        {
            Name = "Test Task",
            Description = "This is a test task"
        };

        var taskId = await SendAsync(createTaskCommand);

        var stopTaskSessionCommand = new StopTaskSessionCommand()
        {
            TaskId = taskId
        };

        await FluentActions.Invoking(() =>
            SendAsync(stopTaskSessionCommand)).Should().ThrowAsync<NoStartedTaskSessionWasFoundException>();
    }
}
