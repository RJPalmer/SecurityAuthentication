using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using SafeVault.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace SafeVault.Pages;

public class IndexModel : PageModel
{
    private readonly Models.AppDbContext _context;

    private readonly SignInManager<User> _signInManager;

    public IndexModel(Models.AppDbContext context, SignInManager<User> signInManager)
    {
        _context = context;
        _signInManager = signInManager;
    }


    [BindProperty]
    public string Username { get; set; }

    [BindProperty]
    public string Email { get; set; }

    public List<User> Users { get; set; }

    public IActionResult OnGet()
    {
        Users = _context.Users.ToList();
        return Page();
    }

    public async Task<IActionResult> OnPostLogoutAsync()
    {
        await _signInManager.SignOutAsync();
        return RedirectToPage("/Account/Logout");
    }
}
