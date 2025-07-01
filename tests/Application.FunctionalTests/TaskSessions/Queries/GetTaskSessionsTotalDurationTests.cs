using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTimeTracker.Application.TaskItems.Commands.CreateTaskItem;
using TaskTimeTracker.Application.TaskSessions.Commands.StartTaskSession;
using TaskTimeTracker.Application.TaskSessions.Commands.StopTaskSession;
using TaskTimeTracker.Application.TaskSessions.Queries.GetTaskSessionDuration;
using TaskTimeTracker.Application.TaskSessions.Queries.GetTaskSessionsTotalDuration;
using static TaskTimeTracker.Application.FunctionalTests.Testing;

namespace TaskTimeTracker.Application.FunctionalTests.TaskSessions.Queries;

public class GetTaskSessionsTotalDurationTests : BaseTestFixture
{
    [Test]
    public async Task ShouldHaveADurationOfThirtySeconds()
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

        await Task.Delay(TimeSpan.FromSeconds(10));

        await SendAsync(stopSessionCommand);

        await SendAsync(startSessionCommand);

        await Task.Delay(TimeSpan.FromSeconds(10));

        await SendAsync(stopSessionCommand);

        await SendAsync(startSessionCommand);

        await Task.Delay(TimeSpan.FromSeconds(10));

        await SendAsync(stopSessionCommand);

        var getTaskSessionDuration = new GetTaskSessionsTotalDurationQuery()
        {
            TaskId = taskId
        };

        var duration = await SendAsync(getTaskSessionDuration);
        duration.Should().Be(TimeSpan.FromSeconds(30).Seconds);
    }

}
