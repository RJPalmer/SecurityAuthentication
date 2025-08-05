using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SafeVault.Models;
using SafeVault.Areas.Identity.Data;
using System.Security.Claims;

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
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
    options.Cookie.Name = "SafeVaultAuthCookie";
    options.Cookie.IsEssential = true; // Required for non-authenticated users to access static files   
});

// EF Core with Identity
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<User>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // Change to true if email confirmation is implemented
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.User.RequireUniqueEmail = true;
})
.AddRoles<AccountRole>() // Add roles support
.AddRoleManager<RoleManager<AccountRole>>() // Register RoleManager
.AddDefaultUI() // Uses built-in Identity Razor Pages
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Fallback authorization policy
builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
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
             var roles = context.User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value);
            logger.LogInformation("User '{Name}' roles: {Roles}", user.Name, string.Join(",", roles));
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

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<AccountRole>>();
    var userManager = services.GetRequiredService<UserManager<User>>();

    string[] roleNames = { "Admin", "User" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new AccountRole { Name = roleName });
        }
    }

    // Assign the Admin role to a specific user
    var adminEmail = "test@test.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser != null && !await userManager.IsInRoleAsync(adminUser, "Admin"))
    {
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}

app.Run();