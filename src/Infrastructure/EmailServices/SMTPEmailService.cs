using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using MimeKit;
using Microsoft.Extensions.Options;

namespace TaskTimeTracker.Infrastructure.EmailServices;

public class SMTPEmailService : IEmailService
{
    private readonly SMTPConfiguration _smtpConfiguration;

    public SMTPEmailService(IOptions<SMTPConfiguration> smtpConfiguration)
    {
        _smtpConfiguration = smtpConfiguration.Value;
    }

    public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("TaskTimeTracker", "mail@vladimirmazarakis.com"));
        message.To.Add(new MailboxAddress("", to));

        message.Subject = subject;
        message.Body = new TextPart(isHtml ? "html" : "plain")
        {
            Text = body
        };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(_smtpConfiguration.Host, _smtpConfiguration.Port, MailKit.Security.SecureSocketOptions.StartTls);

            await client.AuthenticateAsync(_smtpConfiguration.Username, _smtpConfiguration.Password);

            await client.SendAsync(message);

            await client.DisconnectAsync(true);
        }
    }
}
