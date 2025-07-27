using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using BCrypt.Net;
namespace SafeVault.Pages;

[AllowAnonymous]
public class LoginModel : PageModel
{
    public string ReturnUrl { get; set; }

    [BindProperty]
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [BindProperty]
    [Required]
    public string Password { get; set; }

    private readonly AppDbContext _context;

    public LoginModel(AppDbContext context)
    {
        _context = context;
        Email = string.Empty;
        Password = string.Empty;
        ReturnUrl = string.Empty;
    }

    public async Task<IActionResult> OnGetAsync(string returnUrl = null){
        ReturnUrl = returnUrl ?? Url.Content("~/");

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        ReturnUrl = returnUrl ?? Url.Content("~/");
        if (!ModelState.IsValid)
            return Page();

        // Validate user credentials (replace with your actual logic)
        var user = _context.Users.FirstOrDefault(u => u.UserEmail == Email);
        if (user != null && BCrypt.Net.BCrypt.Verify(Password, user.UserPassword))
        {
            // Create claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.UserEmail)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            // Redirect to the originally requested page or Index
            return RedirectToPage("/Index");
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return Page();
    }
    
}