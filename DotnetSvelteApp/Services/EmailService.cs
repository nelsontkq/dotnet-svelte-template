using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using DotnetSvelteApp;
using DotnetSvelteApp.Models.Data;

public interface IEmailService
{
    Task<string> SendVerificationEmail(ApplicationUser user, string token);
}

public class EmailService : IEmailService
{
    private readonly SmtpClient _client;
    private readonly SmtpConfiguration _config;
    private readonly LinkGenerator _linkGenerator;
    private readonly IHttpContextAccessor _accessor;

    public EmailService(
        SmtpClient client,
        IOptions<SmtpConfiguration> config,
        LinkGenerator linkGenerator,
        IHttpContextAccessor accessor)
    {
        _client = client;
        _config = config.Value;
        _linkGenerator = linkGenerator;
        _accessor = accessor;
    }

    public Task<string> SendVerificationEmail(ApplicationUser user, string token)
    {
        var message = new MimeMessage();
        message.Subject = "Verify your email for DotnetSvelteApp";
        message.Sender = MailboxAddress.Parse(_config.Sender);
        message.From.Add(message.Sender);
        message.To.Add(MailboxAddress.Parse(user.Email));
        var action = nameof(AuthenticateController.EmailVerification);
        var url = _linkGenerator.GetUriByAction(_accessor.HttpContext!,
            action,
            values: new
            {
                email = user.Email,
                token,
            });
        if (url is null)
        {
            throw new ApplicationException("Failed to resolve Email Verification URL");
        }

        message.Body = new TextPart("html") { Text = EmailTemplateEngine.GetVerificationEmail(user.UserName, url) };
        return _client.SendAsync(message);
    }
}