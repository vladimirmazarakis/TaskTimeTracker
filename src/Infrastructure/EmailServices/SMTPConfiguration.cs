using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTimeTracker.Infrastructure.EmailServices;
public class SMTPConfiguration
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 587;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
