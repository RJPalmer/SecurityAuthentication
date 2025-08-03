using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

/// <summary>
/// Represents an account role in the SafeVault application.
/// Inherits from IdentityRole to include built-in identity features.
/// Contains properties for the account role ID and role name.
/// Includes a constructor for initializing the role name.
/// Provides a method for creating a new account role asynchronously.
/// </summary>

namespace SafeVault.Models;
[Table("AccountRoles")]
public class AccountRole : IdentityRole<int>
{

    public virtual ICollection<UserAccountRole> UserAccountRoles { get; set; } = new List<UserAccountRole>();
    public async Task<IdentityResult> CreateAsync(RoleManager<AccountRole> roleManager)
    {
        return await roleManager.CreateAsync(this);
    }
}