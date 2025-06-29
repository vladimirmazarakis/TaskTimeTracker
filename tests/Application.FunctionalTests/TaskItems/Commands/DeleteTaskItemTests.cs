using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTimeTracker.Application.Common.Exceptions;
using TaskTimeTracker.Application.Common.Interfaces;
using TaskTimeTracker.Application.TaskItems.Commands.CreateTaskItem;
using TaskTimeTracker.Application.TaskItems.Commands.DeleteTaskItem;
using TaskTimeTracker.Domain.Exceptions.TaskItem;
using static TaskTimeTracker.Application.FunctionalTests.Testing;

namespace TaskTimeTracker.Application.FunctionalTests.TaskItems.Commands;

public class DeleteTaskItemTests : BaseTestFixture
{
    public DeleteTaskItemTests()
    {
    }

    [Test]
    public async Task ShouldThrowExceptionDueToNonExistantTask()
    {
        var user = await RunAsAdministratorAsync();

        var command = new DeleteTaskItemCommand()
        {
            Id = 9999
        };

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<TaskItemNotFoundException>();
    }

    [Test]
    public async Task ShouldDeleteTask()
    {
        var user = await RunAsAdministratorAsync();

        var command = new CreateTaskItemCommand()
        {
            Name = "Test Task",
            Description = "This is a test task",
            Priority = Domain.Enums.PriorityLevel.Medium,
        };

        var result = await SendAsync(command);
        
        var deleteCommand = new DeleteTaskItemCommand()
        {
            Id = result
        };

        var deleteResult = await SendAsync(deleteCommand);

        deleteResult.Should().Be(result);
    }
}
