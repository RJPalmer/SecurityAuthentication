using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SafeVault.Pages;

public class IndexModel : PageModel
{
    private readonly AppDbContext _context;

    public IndexModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public string Username { get; set; }

    [BindProperty]
    public string Email { get; set; }

    public List<User> Users { get; set; }

    public void OnGet()
    {
        Users = _context.Users.ToList();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var user = new User
        {
            UserName = Username,
            UserEmail = Email
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return RedirectToPage(); // Refresh or redirect as needed
    }
}
