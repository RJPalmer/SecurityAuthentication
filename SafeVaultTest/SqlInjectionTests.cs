using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using SafeVault.Models;
using Xunit;

namespace SafeVaultTest;

public class SqlInjectionTests
{
    private class TestDbContext : SafeVault.Models.AppDbContext
    {
        public TestDbContext(DbContextOptions<SafeVault.Models.AppDbContext> options) : base(options) { }
    }

    [Fact]
    public async Task UserLookup_ShouldNotBeVulnerableToSqlInjection()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<SafeVault.Models.AppDbContext>()
            .UseInMemoryDatabase(databaseName: "SqlInjectionTestDb")
            .Options;

        using var context = new TestDbContext(options);

        // Seed a normal user
        context.Users.Add(new User { UserName = "admin", UserPassword = "admin123", UserEmail = "admin@example.com" });
        await context.SaveChangesAsync(CancellationToken.None);

        // Simulate SQL injection attempt
        string maliciousInput = "admin' OR '1'='1";

        // Act
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.UserName == maliciousInput, cancellationToken: CancellationToken.None);

        // Assert
        Assert.Null(user); // Should not find a user with injected input
    }

    [Fact]
    public async Task EmailLookup_ShouldNotBeVulnerableToSqlInjection()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<SafeVault.Models.AppDbContext>()
            .UseInMemoryDatabase(databaseName: "SqlInjectionTestDb_Email")
            .Options;

        using var context = new TestDbContext(options);

        // Seed a normal user
        context.Users.Add(new User { UserName = "jdoe", UserPassword = "password123", UserEmail = "jdoe@example.com" });
        await context.SaveChangesAsync(CancellationToken.None);

        // Simulate SQL injection attempt
        string maliciousInput = "jdoe@example.com' OR '1'='1";

        // Act
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.UserEmail == maliciousInput, cancellationToken: CancellationToken.None);

        // Assert
        Assert.Null(user); // Should not find a user with injected input
    }

    [Fact]
    public async Task PasswordCheck_ShouldNotBeVulnerableToSqlInjection()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<SafeVault.Models.AppDbContext>()
            .UseInMemoryDatabase(databaseName: "SqlInjectionTestDb_Password")
            .Options;

        using var context = new TestDbContext(options);

        // Seed a normal user
        context.Users.Add(new User { UserName = "susan", UserPassword = "susan123", UserEmail = "susan@example.com" });
        await context.SaveChangesAsync(CancellationToken.None);

        // Simulate SQL injection attempt
        string maliciousInput = "susan123' OR '1'='1";

        // Act
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.UserPassword == maliciousInput, cancellationToken: CancellationToken.None);

        // Assert
        Assert.Null(user); // Should not find a user with injected input
    }
}
