using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using SafeVault.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace SafeVault.Pages;

public class IndexModel : PageModel
{
    private readonly AppDbContext _context;

    private readonly SignInManager<IdentityUser> _signInManager;

    public IndexModel(AppDbContext context, SignInManager<IdentityUser> signInManager)
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
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToPage("/Account/Login");
    }
}
