using Microsoft.EntityFrameworkCore;

public class FinRecDbContext(DbContextOptions<FinRecDbContext> options) : DbContext(options)
{
    public DbSet<SafeVault.Models.FinancialRecord> FinancialRecord { get; set; } = default!;
}
