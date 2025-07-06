using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskTimeTracker.Domain.Entities;

namespace TaskTimeTracker.Infrastructure.Data.Configurations;

public class TaskSessionConfiguration : IEntityTypeConfiguration<TaskSession>
{
    public void Configure(EntityTypeBuilder<TaskSession> builder)
    {
        builder.HasOne(ts => ts.Task)
            .WithMany(ti => ti.TaskSessions)
            .HasForeignKey(ts => ts.TaskId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
