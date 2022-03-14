using System.Xml.Linq;

namespace DotnetSvelteApp;

// TODO: replace with a real template engine
public static class EmailTemplateEngine
{
    private static readonly string VERIFICATION_EMAIL_TEMPLATE = XDocument.Parse(@"
<html>
    <body>
        <h1>
            Hi {{UserName}},
        </h1>
        <p>
            Thank you for registering with us. Please click on the link below to verify your email address.
        </p>
        <p><a href=""{{VerificationLink}}"">Verify Email</a></p>
        <p>
            If you did not register with us, please ignore this email.
        </p>
    </body>
    <style>
    h1 {
        font-size: 2em;
        font-weight: bold;
        text-align: center;
    }
    p {
        font-size: 1.2em;
        text-align: center;
    }
    </style>
    </html>").ToString(SaveOptions.None);

    public static string GetVerificationEmail(string userName, string verificationLink)
    {
        return VERIFICATION_EMAIL_TEMPLATE
            .Replace("{{UserName}}", userName)
            .Replace("{{VerificationLink}}", verificationLink);
    }
}