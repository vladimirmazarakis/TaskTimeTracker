using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTimeTracker.Application.TaskItems.Commands.CreateTaskItem;
using TaskTimeTracker.Application.TaskItems.Commands.UpdateTaskItem;
using TaskTimeTracker.Domain.Exceptions.TaskItem;
using static TaskTimeTracker.Application.FunctionalTests.Testing;

namespace TaskTimeTracker.Application.FunctionalTests.TaskItems.Commands;

public class UpdateTaskItemTests : BaseTestFixture
{
    public UpdateTaskItemTests()
    {
    }

    [Test]
    public async Task ShouldThrowExceptionDueToNullAndEmptyProperties()
    {
        var user = await RunAsAdministratorAsync();
        var command = new UpdateTaskItemCommand()
        {
        };
        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<Exception>();
    }

    [Test]
    public async Task ShouldThrowExceptionDueToNonExistantItem()
    {
        var user = await RunAsAdministratorAsync();
        var command = new UpdateTaskItemCommand()
        {
            Id= 9999,
            Name = "Test Task",
            Description = "This is a test task",
            Priority = Domain.Enums.PriorityLevel.Medium,
            IsCompleted = false
        };
        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<TaskItemNotFoundException>();
    }

    [Test]
    public async Task ShouldUpdate()
    {
        var user = await RunAsAdministratorAsync();
        var command = new CreateTaskItemCommand()
        {
            Name = "Test Task",
            Description = "This is a test task",
            Priority = Domain.Enums.PriorityLevel.Medium,
        };
        var id = await SendAsync(command);

        id.Should().BeGreaterThan(-1);

        var updateCommand = new UpdateTaskItemCommand()
        {
            Id = id,
            Name = "Updated Task",
            Description = "This is an updated test task",
            Priority = Domain.Enums.PriorityLevel.High,
            IsCompleted = true
        };

        var updated = await SendAsync(updateCommand);

        updated.Should().NotBeNull();
    }
}
