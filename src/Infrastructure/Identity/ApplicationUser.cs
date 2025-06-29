using Microsoft.AspNetCore.Identity;
using TaskTimeTracker.Domain.Entities;

namespace TaskTimeTracker.Infrastructure.Identity;
public class ApplicationUser : IdentityUser
{
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
