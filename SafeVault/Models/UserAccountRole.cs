using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SafeVault.Models;

/// <summary>
/// Represents a mapping between a user and an account role in the SafeVault application.
/// Inherits from IdentityUserRole&lt;int&gt;, which includes UserId and RoleId as composite keys.
/// </summary>
[Table("UserAccountRoles")]

public class UserAccountRole : IdentityUserRole<int>
{
    /// <summary>
    /// Gets or sets the UserId, which is a foreign key to the User model.
    /// </summary>
    [ForeignKey("User")]
    public int UserId { get; set; } // Foreign key to User
    [ForeignKey("AccountRole")]
    public int RoleId { get; set; } // Foreign key to AccountRole
    public virtual User ApplicationUser { get; set; } = null!; // Navigation property to User
    public virtual AccountRole ApplicationRole { get; set; } = null!; // Navigation property to AccountRole

}