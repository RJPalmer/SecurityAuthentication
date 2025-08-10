using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SafeVault.Models;

public class DashboardModel : PageModel
{
    public string UserName { get; set; } = "user";
    public DateTime LastLogin { get; set; } = DateTime.Now.AddDays(-1);
    public string SecurityStatus { get; set; } = "2FA enabled";
    public int TotalItems { get; set; } = 56;
    public DateTime LastUpdated { get; set; } = DateTime.Now.AddDays(-3);
    public int RecentActivityCount { get; set; } = 3;

    private readonly SafeVault.Models.AppDbContext _context;

    private readonly SignInManager<User> _signInManager;

    public DashboardModel(SafeVault.Models.AppDbContext context, SignInManager<User> signInManager)
    {
        _context = context;
        _signInManager = signInManager;

        // Initialize any additional properties or services here
        UserName = _signInManager.Context.User.Identity?.Name ?? "Guest";
        LastLogin = DateTime.Now; // This would typically be fetched from the database
        SecurityStatus = "2FA enabled"; // This would also typically be fetched from the database
        TotalItems = _context.Users.Count(); // Example of fetching total items from the database
        LastUpdated = DateTime.Now; // This would typically be the last update timestamp from the database
        RecentActivityCount = 3; // This could be dynamically calculated based on user activity 


    }
    
    public Dictionary<string, int> VaultSummary { get; set; } = new()
    {
        { "Passwords", 24 },
        { "Bank Accounts", 6 },
        { "Credit Cards", 12 },
        { "Secure Notes", 8 },
        { "Identity Documents", 6 }
    };

    public List<string> RecentActivities { get; set; } = new()
    {
        "Logged in - Today, 10:32 AM",
        "Viewed Financial record - Yesterday, 11:18 PM",
        "Edited Login item - Yesterday, 4:25 PM",
        "Logged in - Apr 21"
    };

    public List<User> Users { get; set; } = new();

    public List<string> SecurityAlerts { get; set; } = new()
    {
        "Your Gmail account was in a data breach",
        "Weak password for Dropbox"
    };

    public IActionResult OnGet()
    {
        // Any additional logic goes here
        Users = _context.Users.ToList();
        return Page();
    }
}