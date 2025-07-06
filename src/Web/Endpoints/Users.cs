using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using TaskTimeTracker.Infrastructure.EmailServices;
using TaskTimeTracker.Infrastructure.Identity;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace TaskTimeTracker.Web.Endpoints;
public class Users : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        var group = app.MapGroup(this);

        group.MapIdentityApi<ApplicationUser>();

        group.MapGet("/confirmEmailRedirect", ConfirmEmail);
    }

    public async Task<IResult> ConfirmEmail(string userId, string code, UserManager<ApplicationUser> userManager, IOptions<EmailLinksConfiguration> emailLinksConfiguration)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
        {
            return Results.BadRequest("Invalid confirmation link");
        }

        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return Results.BadRequest("Invalid user ID");
        }
        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        var result = await userManager.ConfirmEmailAsync(user, code);

        if (result.Succeeded)
        {
            return Results.Redirect(emailLinksConfiguration.Value.ConfirmAccountUrl);
        }

        return Results.BadRequest("Email confirmation failed");
    }
}
