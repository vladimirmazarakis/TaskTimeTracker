using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using TaskTimeTracker.Domain.Entities;

namespace TaskTimeTracker.Domain.UnitTests.Entities
{
    public class TaskSessionTests
    {
        [Test]
        public void ShouldReturnOneHourInSeconds() 
        {
            TaskSession taskSession = new TaskSession();

            var today = DateTime.UtcNow;

            taskSession.StartDateTime = today;
            taskSession.EndDateTime = today.AddHours(1);

            taskSession.DurationInSeconds().Should().Be(3600);
        }

        [Test]
        public void ShouldReturnTwentyFourHoursInSeconds()
        {
            TaskSession taskSession = new TaskSession();

            var today = DateTime.UtcNow;

            taskSession.StartDateTime = today;
            taskSession.EndDateTime = today.AddHours(24);

            taskSession.DurationInSeconds().Should().Be(86400);
        }
    }
}
