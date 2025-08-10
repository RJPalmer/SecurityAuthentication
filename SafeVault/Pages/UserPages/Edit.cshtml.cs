using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SafeVault.Models;

namespace SafeVault.Pages.UserPages;

public class EditModel : PageModel
{
    private readonly Models.AppDbContext _context;
    private readonly RoleManager<AccountRole> _roleManager;

    private readonly UserManager<User> _userManager;
    public EditModel(Models.AppDbContext context, RoleManager<AccountRole> roleManager, UserManager<User> userManager)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    [BindProperty]
    public User Users { get; set; } = default!;

    public IList<string> Roles { get; set; } = new List<string>();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var user = await _userManager.Users.FirstOrDefaultAsync(m => m.Id == id);
        if (user is null)
        {
            return NotFound();
        }
        Users = user;
        Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var editUser = await _userManager.FindByIdAsync(Users.Id.ToString());
        if (editUser == null)
        {
            return NotFound();
        }
    

        // Assuming you have a way to get the role ID from the selected role name
        var roleName = Request.Form["Users.UserAccountRoles"];
        var userRole = await _userManager.GetRolesAsync(Users);
        if (userRole.Count > 0)
        {
            await _userManager.RemoveFromRoleAsync(editUser, userRole[0]);
        }
        if (string.IsNullOrEmpty(roleName))
        {
            ModelState.AddModelError("Users.UserAccountRoles", "Role is required.");
            return Page();
        }
        // Find the role by name
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            ModelState.AddModelError("Users.UserAccountRoles", "Role not found.");
            return Page();
        }
        var roleId = role.Id;

        // Update user properties
        editUser.UserName = Users.UserName;
        editUser.Email = Users.Email;
        editUser.UserPassword = Users.UserPassword;
        editUser.PasswordHash = _userManager.PasswordHasher.HashPassword(editUser, Users.UserPassword);

        _userManager.AddToRoleAsync(editUser, role.Name).Wait();
        _userManager.UpdateAsync(editUser).Wait();


        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(Users.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private bool UserExists(int id)
    {
        return _context.Users.Any(e => e.Id == id);
    }
}
