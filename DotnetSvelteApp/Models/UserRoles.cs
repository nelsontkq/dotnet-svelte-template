namespace DotnetSvelteAuthApp.Models;

public static class UserRoles
{
    public const string Admin = "Admin";
    public const string User = "User";
    public static IEnumerable<string> AllRoles => new[] { Admin, User };
}