using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTimeTracker.Infrastructure.EmailServices;
public class EmailLinksConfiguration
{
    public string LoginUrl { get; set; } = string.Empty;
    public string ConfirmAccountUrl { get; set; } = string.Empty;
    /// <summary>
    /// Must contain CODE_LOC and EMAIL_LOC, to be replaced by the code needed to reset the password and the email. For example: http://localhost:5173/auth/forgot-pass?email=EMAIL_LOC&code=CODE_LOC <= THIS
    /// </summary>
    public string ForgotPasswordUrl { get; set; } = string.Empty;
}
