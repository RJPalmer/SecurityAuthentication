using Microsoft.EntityFrameworkCore;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<SafeVault.Models.User> User { get; set; } = default!;
    public object UserAccountRoles { get; internal set; }
}
