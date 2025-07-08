using System.Reflection;
using System.Runtime.CompilerServices;
using AutoMapper;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TaskTimeTracker.Application.Common.Interfaces;
using TaskTimeTracker.Application.Common.Models;
using TaskTimeTracker.Application.TaskItems.Commands.UpdateTaskItem;
using TaskTimeTracker.Application.TaskItems.Queries;
using TaskTimeTracker.Application.TaskSessions.Queries;
using TaskTimeTracker.Domain.Entities;

namespace TaskTimeTracker.Application.UnitTests.Common.Mappings;
public class MappingTests
{
    private readonly IConfigurationProvider _configuration;
    private readonly IMapper _mapper;

    public MappingTests()
    {
        _configuration = new MapperConfiguration(config =>
            config.AddMaps(Assembly.GetAssembly(typeof(IApplicationDbContext))));

        _mapper = _configuration.CreateMapper();
    }

    [Test]
    public void ShouldHaveValidConfiguration()
    {
        _configuration.AssertConfigurationIsValid();
    }

    [Test]
    [TestCase(typeof(TaskItem), typeof(TaskItemDto))]
    [TestCase(typeof(List<TaskItem>), typeof(TaskItemsDto))]
    [TestCase(typeof(TaskItem), typeof(UpdatedTaskItemDto))]
    [TestCase(typeof(List<TaskSession>), typeof(TaskSessionsDto))]
    [TestCase(typeof(TaskSession), typeof(TaskSessionDto))]
    public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
    {
        var instance = GetInstanceOf(source);

        _mapper.Map(instance, source, destination);
    }

    private object GetInstanceOf(Type type)
    {
        if (type.GetConstructor(Type.EmptyTypes) != null)
            return Activator.CreateInstance(type)!;

        // Type without parameterless constructor
        return RuntimeHelpers.GetUninitializedObject(type);
    }
}
