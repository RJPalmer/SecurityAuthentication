using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SafeVault.Models;

namespace SafeVault.Pages.UserPages;

public class DeleteModel : PageModel
{
    private readonly Models.AppDbContext _context;

    public DeleteModel(Models.AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public User Users { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
        if (user is null)
        {
            return NotFound();
        }
        else
        {
            Users = user;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            Users = user;
            _context.Users.Remove(Users);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
