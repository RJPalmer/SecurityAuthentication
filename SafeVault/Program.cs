using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SafeVault.Models;
using SafeVault.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

// --------------------------------------------
// SERVICES
// --------------------------------------------

// Razor Pages with global authorization policy, but allow Identity pages
builder.Services.AddRazorPages(options =>
{   
    options.Conventions.AuthorizeFolder("/"); // Require auth for all Razor pages
    options.Conventions.AllowAnonymousToAreaFolder("Identity", "/Account"); // Allow login/register/etc
});

// Cookie settings for authentication
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";

    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
});

// EF Core with Identity
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity with custom User and Role models
builder.Services.AddIdentity<User, AccountRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
})
.AddDefaultUI() // Uses built-in Identity Razor Pages
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Fallback authorization policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAuthenticatedUser", policy =>
        policy.RequireAuthenticatedUser());

    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// Logging
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// --------------------------------------------
// APP PIPELINE
// --------------------------------------------

var app = builder.Build();
var logger = app.Services.GetRequiredService<ILogger<Program>>();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();

// Logging middleware
app.Use(async (context, next) =>
{
    logger.LogInformation("Incoming request: {Method} {Path}", context.Request.Method, context.Request.Path);

    try
    {
        await next();

        var user = context.User.Identity;
        if (user?.IsAuthenticated == true)
        {
            logger.LogInformation("User '{Name}' is authenticated", user.Name);
        }
        else
        {
            logger.LogInformation("Request was anonymous");
        }

        if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
        {
            logger.LogWarning("Authorization failed: access denied for {Path}", context.Request.Path);
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Unhandled exception during request to {Path}", context.Request.Path);
        throw;
    }
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();