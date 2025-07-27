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

    public IActionResult OnGet()
    {
        Users = _context.Users.ToList();
        return Page();
    }
}
