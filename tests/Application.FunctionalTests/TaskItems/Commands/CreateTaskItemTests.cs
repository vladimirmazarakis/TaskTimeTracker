using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTimeTracker.Application.Common.Exceptions;
using TaskTimeTracker.Application.Common.Interfaces;
using TaskTimeTracker.Application.TaskItems.Commands.CreateTaskItem;

using static TaskTimeTracker.Application.FunctionalTests.Testing;

namespace TaskTimeTracker.Application.FunctionalTests.TaskItems.Commands;

public class CreateTaskItemTests : BaseTestFixture
{
    public CreateTaskItemTests()
    {
    }

    [Test]
    public async Task ShouldThrowExceptionDueToNullAndEmptyProperties()
    {
        var user = await RunAsAdministratorAsync();

        var command = new CreateTaskItemCommand()
        {
        };

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<Exception>();
    }

    [Test]
    public async Task ShouldCreateTask()
    {
        var user = await RunAsAdministratorAsync();

        var command = new CreateTaskItemCommand()
        {
            Name = "Test Task",
            Description = "This is a test task",
            Priority = Domain.Enums.PriorityLevel.Medium,
        };

        var result = await SendAsync(command);
        result.Should().BeGreaterThan(-1);
    }
}
