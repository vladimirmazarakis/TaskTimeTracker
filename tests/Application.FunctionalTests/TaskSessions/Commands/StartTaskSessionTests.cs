using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTimeTracker.Application.TaskItems.Commands.CreateTaskItem;
using TaskTimeTracker.Application.TaskSessions.Commands.StartTaskSession;
using TaskTimeTracker.Domain.Exceptions.TaskItem;
using TaskTimeTracker.Domain.Exceptions.TaskSession;
using static TaskTimeTracker.Application.FunctionalTests.Testing;

namespace TaskTimeTracker.Application.FunctionalTests.TaskSessions.Commands;
public class StartTaskSessionTests : BaseTestFixture
{
    [Test]
    public async Task ShouldStartTaskSession()
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

        var result = await SendAsync(startSessionCommand);
        
        result.Should().BeTrue();
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

        var startSessionCommand = new StartTaskSessionCommand()
        {
            TaskId = 999
        };

        await FluentActions.Invoking(() =>
            SendAsync(startSessionCommand)).Should().ThrowAsync<TaskItemNotFoundException>();
    }

    [Test]
    public async Task ShouldThrowStartedTaskSessionAlreadyExistsException()
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

        await SendAsync(startSessionCommand);

        await FluentActions.Invoking(() =>
            SendAsync(startSessionCommand)).Should().ThrowAsync<StartedTaskSessionAlreadyExistsException>();
    }
}
