using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;

namespace DotnetSvelteAuthApp.Models.Data;

/// <inheritdoc />
public class ApplicationUser : IdentityUser
{
    private const int ID_LENGTH = 6;

    public override string UserName { get => base.UserName[..^(ID_LENGTH + 1)]; set => base.UserName = value + "#" + RandomChars(ID_LENGTH); }


    private static string RandomChars(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Shared.Next(s.Length)]).ToArray());
    }
}