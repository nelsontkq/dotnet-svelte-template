namespace DotnetSvelteApp.Utility.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ILogger<ExceptionHandlingMiddleware> logger)
    {
        try
        {
            await _next(context);
        }
        catch (PubliclyVisibleException ex)
        {
            context.Response.StatusCode = 400;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(ex.Message);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            var uniqueID = GenerateUniqueString(5);
            logger.LogCritical(ex, $"Unhandled exception! {uniqueID}");
            await context.Response.WriteAsync($"Internal server error occurred. Contact support with the following ID: '{uniqueID}'");
        }
    }
    private string GenerateUniqueString(int length)
    {
        const string chars = "ABCDEFGHJKMNPQRSTUVWXYZ123456789abcdefghijkmnpqrstuvwxyz";
        var sb = new StringBuilder(length);
        for (var i = 0; i < length; i++)
        {
            var randomIndex = Random.Shared.Next(0, chars.Length - 1);
            sb.Append(chars[randomIndex]);
        }
        return sb.ToString();
    }
}