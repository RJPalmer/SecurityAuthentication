using System.ComponentModel.DataAnnotations;

namespace SafeVault.Models;

/// <summary>
/// Represents a credential stored in the SafeVault application.
/// Contains properties for the credential ID, user ID, label, username, encrypted password, URL
/// and creation timestamp.
/// Includes a navigation property to the User model for relational mapping.
/// </summary>

public class Credential
{
    public int CredentialID { get; set; }

    [Required]
    public int UserID { get; set; }

    [Required]
    [MaxLength(100)]
    public string Label { get; set; }

    public string Username { get; set; }

    [Required]
    public string EncryptedPassword { get; set; }

    public string Url { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; }
}