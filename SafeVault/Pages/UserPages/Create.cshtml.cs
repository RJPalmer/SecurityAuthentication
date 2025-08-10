using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SafeVault.Models;

namespace SafeVault.Pages.UserPages;

public class CreateModel : PageModel
{
    private readonly Models.AppDbContext _context;

    public CreateModel(Models.AppDbContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public User Users { get; set; } = default!;

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Users.Add(Users);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
