using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace SafeVault.Models;

public class AppDbContext : IdentityDbContext<User, AccountRole, int>
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .ToTable("Users");
        modelBuilder.Entity<User>()
            .Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(100);
        modelBuilder.Entity<User>()
            .Property(u => u.UserEmail)
            .IsRequired()
            .HasMaxLength(100);
        modelBuilder.Entity<User>()
            .Property(u => u.UserPassword)
            .IsRequired()
            .HasMaxLength(100);
        modelBuilder.Entity<User>()
            .HasIndex(u => u.UserEmail)
            .IsUnique();
        modelBuilder.Entity<User>()
            .HasMany(u => u.UserAccountRoles)
            .WithOne(uar => uar.ApplicationUser)
            .HasForeignKey(uar => uar.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<AccountRole>()
            .ToTable("AccountRoles");
        modelBuilder.Entity<AccountRole>()
            .HasIndex(ar => ar.Name)
            .IsUnique();
        modelBuilder.Entity<AccountRole>()
            .HasMany(ar => ar.UserAccountRoles)
            .WithOne(uar => uar.ApplicationRole)
            .HasForeignKey(uar => uar.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<UserAccountRole>()
            .ToTable("UserAccountRoles");
        modelBuilder.Entity<UserAccountRole>()
            .HasOne(uar => uar.ApplicationUser)
            .WithMany(u => u.UserAccountRoles)
            .HasForeignKey(uar => uar.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        modelBuilder.Entity<UserAccountRole>()
            .HasOne(uar => uar.ApplicationRole)
            .WithMany(ar => ar.UserAccountRoles)
            .HasForeignKey(uar => uar.RoleId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        base.OnModelCreating(modelBuilder);
        // Additional model configurations can go here

    }
    public new DbSet<User> Users { get; set; }

    public  DbSet<AccountRole> AccountRoles { get; set; }

    public  DbSet<UserAccountRole> UserAccountRoles { get; set; }
}

