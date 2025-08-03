using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SafeVault.Models;

/// <summary>
/// Represents a financial record in the SafeVault application.
/// Contains properties for the financial record ID, user ID, type, account name,
/// encrypted details, and creation timestamp.
/// Includes a navigation property to the User model for relational mapping.
/// </summary>  


public class FinancialRecord
{
    public int FinancialRecordID { get; set; }

    [Required]
    public int UserID { get; set; }

    [Required]
    [MaxLength(100)]
    public string Type { get; set; }

    [Required]
    [MaxLength(100)]
    public string AccountName { get; set; }

    [Required]
    public string EncryptedDetails { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; }
}