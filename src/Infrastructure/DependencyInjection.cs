using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using TaskTimeTracker.Application.Common.Interfaces;
using TaskTimeTracker.Domain.Constants;
using TaskTimeTracker.Infrastructure.Data;
using TaskTimeTracker.Infrastructure.Data.Interceptors;
using TaskTimeTracker.Infrastructure.EmailServices;
using TaskTimeTracker.Infrastructure.Identity;
using TaskTimeTracker.Infrastructure.Identity.EmailSenders;

namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("TaskTimeTrackerDb");
        Guard.Against.Null(connectionString, message: "Connection string 'TaskTimeTrackerDb' not found.");

        builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        builder.Services.AddScoped<ISaveChangesInterceptor, TaskItemEntityInterceptor>();

        builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlServer(connectionString).AddAsyncSeeding(sp);
        });

        builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        builder.Services.AddScoped<ApplicationDbContextInitialiser>();

        builder.Services.AddAuthentication()
            .AddBearerToken(IdentityConstants.BearerScheme, (options) => 
            {
                options.BearerTokenExpiration = TimeSpan.FromMinutes(60);
            });

        builder.Services.AddAuthorizationBuilder();

        builder.Services
            .AddIdentityCore<ApplicationUser>((options) => 
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddApiEndpoints();

        builder.Services.AddSingleton(TimeProvider.System);
        builder.Services.AddTransient<IIdentityService, IdentityService>();

        builder.Services.Configure<SMTPConfiguration>(builder.Configuration.GetSection("SMTP"));
        builder.Services.Configure<EmailLinksConfiguration>(builder.Configuration.GetSection("EmailLinks"));

        builder.Services.AddTransient<IEmailService, SMTPEmailService>();
        builder.Services.AddTransient<IEmailSender<ApplicationUser>, EmailSender>();



        builder.Services.AddAuthorization(options =>
            options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator)));
    }
}
