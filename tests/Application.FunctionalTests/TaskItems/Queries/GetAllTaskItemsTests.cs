using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTimeTracker.Application.TaskItems.Commands.CreateTaskItem;
using TaskTimeTracker.Application.TaskItems.Queries.GetAllTaskItems;
using static TaskTimeTracker.Application.FunctionalTests.Testing;

namespace TaskTimeTracker.Application.FunctionalTests.TaskItems.Queries;

public class GetAllTaskItemsTests : BaseTestFixture
{
    public GetAllTaskItemsTests()
    {
        
    }

    [Test]
    public async Task ShouldReturnFiveTaskItems()
    {
        var user = await RunAsAdministratorAsync();

        var createCommand = new CreateTaskItemCommand()
        {
            Name = "Test Task",
            Description = "This is a test task",
            Priority = Domain.Enums.PriorityLevel.Medium,
        };

        for(int i = 0; i < 5; i++)
        {
            await SendAsync(createCommand);
        }

        var query = new GetAllTaskItemsQuery();
        
        var result = await SendAsync(query);
        result.Should().NotBeNull();
        result.TaskItems.Count.Should().Be(5);
    }

    [Test]
    public async Task ShouldReturnZeroTaskItems()
    {
        var user = await RunAsAdministratorAsync();

        var query = new GetAllTaskItemsQuery();

        var result = await SendAsync(query);
        result.Should().NotBeNull();
        result.TaskItems.Count.Should().Be(0);
    }
}
