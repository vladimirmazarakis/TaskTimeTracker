using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using TaskTimeTracker.Application.Common.Behaviours;
using TaskTimeTracker.Application.Common.Interfaces;

namespace TaskTimeTracker.Application.UnitTests.Common.Behaviours;
public class RequestLoggerTests
{
    private Mock<IUser> _user = null!;
    private Mock<IIdentityService> _identityService = null!;

    [SetUp]
    public void Setup()
    {
        _user = new Mock<IUser>();
        _identityService = new Mock<IIdentityService>();
    }
}
