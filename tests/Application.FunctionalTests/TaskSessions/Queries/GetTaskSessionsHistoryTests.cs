using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTimeTracker.Application.TaskItems.Commands.CreateTaskItem;
using TaskTimeTracker.Application.TaskSessions.Commands.StartTaskSession;
using TaskTimeTracker.Application.TaskSessions.Commands.StopTaskSession;
using TaskTimeTracker.Application.TaskSessions.Queries.GetTaskSessionDuration;
using TaskTimeTracker.Application.TaskSessions.Queries.GetTaskSessionsHistory;
using static TaskTimeTracker.Application.FunctionalTests.Testing;

namespace TaskTimeTracker.Application.FunctionalTests.TaskSessions.Queries;
public class GetTaskSessionsHistoryTests : BaseTestFixture
{

    [Test]
    public async Task ShouldHaveASessionHistoryWithACountOfThree()
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
        await SendAsync(stopSessionCommand);
        await SendAsync(startSessionCommand);
        await SendAsync(stopSessionCommand);
        await SendAsync(startSessionCommand);
        await SendAsync(stopSessionCommand);


        var GetTaskSessionsHistoryQuery = new GetTaskSessionsHistoryQuery()
        {
            TaskId = taskId
        };

        var result = await SendAsync(GetTaskSessionsHistoryQuery);
        result.TaskSessions.Count.Should().Be(3);
    }
}
