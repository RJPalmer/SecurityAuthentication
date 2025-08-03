using System.ComponentModel.DataAnnotations;

namespace SafeVault.Models;

/// <summary>
/// Represents an audit log entry in the SafeVault application
/// Contains properties for the audit log ID, user ID, action performed, IP address,
/// details of the action, and timestamp
/// Includes a navigation property to the User model for relational mapping
/// </summary>

public class AuditLog
{
    public int AuditLogID { get; set; }

    public int? UserID { get; set; }

    [Required]
    public string Action { get; set; }

    public string IPAddress { get; set; }

    public string Details { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public User User { get; set; }
}