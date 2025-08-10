using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SafeVault.Models;

namespace SafeVault.Pages.UserPages;

public class DetailsModel : PageModel
{
    private readonly Models.AppDbContext _context;

    private readonly UserManager<User> _userManager;
    public DetailsModel(Models.AppDbContext context, UserManager<User> userManager)
    {
        _userManager = userManager;
        _context = context;
    }

    public User Users { get; set; } = default!;

    public string UserRole { get; set; } = string.Empty;

    public IList<string> Roles { get; set; } = new List<string>();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user is null)
        {
            return NotFound();
        }
        else
        {
            Users = user;
            UserRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "No Role Assigned";
        }

        return Page();
    }
}
