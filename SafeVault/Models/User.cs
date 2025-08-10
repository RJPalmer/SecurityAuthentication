using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace SafeVault.Models;

/// <summary>
/// Represents a user in the SafeVault application.
/// Inherits from IdentityUser to include built-in identity features.
/// Contains properties for user ID, username, email, password, and roles.
/// Includes a collection for user account roles for relational mapping.
/// </summary>
[Table("Users")]
public class User : IdentityUser<int>
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public override int Id { get; set; }


    [Required]
    [MaxLength(100)]
    public required override string UserName { get; set; }

    [Required]
    [MaxLength(100)]
    public required string UserEmail { get; set; }

    public override string? Email { get => UserEmail; set => UserEmail = value; }

    [Required]
    [MaxLength(100)]
    public required string UserPassword { get; set; }

    public override string? PasswordHash { get => UserPassword; set => UserPassword = value; }

    public User()
    {
        base.UserName = string.Empty;
        base.Email = string.Empty;
        base.PasswordHash = string.Empty;
        UserAccountRoles = new List<UserAccountRole>();
    }

    public virtual ICollection<UserAccountRole> UserAccountRoles { get; set; } = new List<UserAccountRole>();
}