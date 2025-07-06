﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTimeTracker.Infrastructure.EmailServices;
public interface IEmailService
{
    public Task SendEmailAsync(string to, string subject, string body, bool isHtml = true);
}
