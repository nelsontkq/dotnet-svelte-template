using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using DotnetSvelteAuthApp.Models.Data;
using DotnetSvelteAuthApp.Utility.Middleware;

var builder = WebApplication.CreateBuilder(args);


// Services
{
    var services = builder.Services;
    services.AddCors();
    services.AddControllersWithViews();
    var configuration = builder.Configuration;
    services.Configure<Jwt>(a => configuration.GetSection("JWT").Bind(a)).AddOptions<Jwt>("JWT").ValidateDataAnnotations();
    services.Configure<SmtpConfiguration>(a => configuration.GetSection("Smtp").Bind(a)).AddOptions<SmtpConfiguration>("Smtp").ValidateDataAnnotations();
    services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
    });
    services.AddDatabaseDeveloperPageExceptionFilter();

    services
        .AddDefaultIdentity<ApplicationUser>(opts =>
        {
            opts.Password.RequiredLength = 10;
            opts.User.AllowedUserNameCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz_-.#";
            opts.User.RequireUniqueEmail = true;
        }).AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>();

    services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = configuration["JWT:ValidAudience"],
            ValidIssuer = configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
        };
    });

    services.AddLogging();
    services.AddSwaggerDocument();
    services.AddSingleton(typeof(SmtpClient), (serv) =>
    {
        var client = new SmtpClient();
        var config = serv.GetService<IOptions<SmtpConfiguration>>()!.Value;

        client.Connect(config.Host, config.Port, MailKit.Security.SecureSocketOptions.StartTls);
        client.Authenticate(config.UserName, config.Password);
        return client;
    });
    services.AddHttpContextAccessor();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IEmailService, EmailService>();
}
var app = builder.Build();

// Middleware
{
    app.UseMiddleware<ExceptionHandlingMiddleware>();
}


// Misc configuration
{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    // Register the Swagger generator and the Swagger UI middlewares
    app.UseOpenApi();
    app.UseSwaggerUi3();

    // Let nginx handle cors
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action=Index}/{id?}");

    app.MapFallbackToFile("index.html"); ;
}

app.Run();
