using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using TaskTimeTracker.Infrastructure.EmailServices;

namespace TaskTimeTracker.Infrastructure.Identity.EmailSenders;
public class EmailSender : IEmailSender<ApplicationUser>
{
    private readonly IEmailService _emailService;
    private readonly EmailLinksConfiguration _emailLinksConfiguration;

    public EmailSender(IEmailService emailService, IOptions<EmailLinksConfiguration> emailLinksConfiguration)
    {
        _emailService = emailService;
        _emailLinksConfiguration = emailLinksConfiguration.Value;
    }

    public async Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
    {
        confirmationLink = confirmationLink.Replace("confirmEmail", "confirmEmailRedirect");

        await _emailService.SendEmailAsync(
            to: email,
            subject: "Confirm your email",
            body: $"Please confirm your email by clicking this link: <a href=\"{confirmationLink}\">Confirm Email</a>",
            isHtml: true
        );
    }

    public async Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
    {
        string resetUrl = _emailLinksConfiguration.ForgotPasswordUrl.Replace("CODE_LOC", resetCode).Replace("EMAIL_LOC", email);
            
        await _emailService.SendEmailAsync(
            to: email,
            subject: "Reset your password",
            body: $"Please reset your password by clicking this link: <a href=\"{resetUrl}\">Reset Password</a>",
            isHtml: true
        );
    }

    public async Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
    {
        await _emailService.SendEmailAsync(
            to: email,
            subject: "Reset your password",
            body: $"Please reset your password by clicking this link: <a href=\"{resetLink}\">Reset Password</a>",
            isHtml: true
        );
    }
}
