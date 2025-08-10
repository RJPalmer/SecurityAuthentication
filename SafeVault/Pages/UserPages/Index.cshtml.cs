using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SafeVault.Models;

namespace SafeVault.Pages.UserPages;

public class IndexModel : PageModel
{
    private readonly SafeVault.Models.AppDbContext _context;

    private readonly UserManager<User> _userManager;

    private readonly RoleManager<AccountRole> _roleManager;


    public IndexModel(SafeVault.Models.AppDbContext context, UserManager<User> userManager, RoleManager<AccountRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public IList<User> Users { get; set; } = default!;
    public IList<string> UserRoles { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Users = await _userManager.Users.ToListAsync();
        UserRoles = new List<string>();
        var test = await _roleManager.Roles.ToListAsync();
        foreach (var user in Users)
        {
            var role = await _userManager.GetRolesAsync(user);
            if(role.Count > 0)
            {
                UserRoles.Add(role[0]);
            }
            else
            {
                UserRoles.Add("No Role Assigned");
            }
            // var roles = list;
            // You can process roles as needed
        }
    }
}
