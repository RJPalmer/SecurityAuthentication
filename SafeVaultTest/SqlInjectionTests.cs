using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using SafeVault.Models;
using Xunit;

namespace SafeVaultTest;

public class SqlInjectionTests
{
    private class TestDbContext : AppDbContext
    {
        public TestDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }

    [Fact]
    public async Task UserLookup_ShouldNotBeVulnerableToSqlInjection()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "SqlInjectionTestDb")
            .Options;

        using var context = new TestDbContext(options);

        // Seed a normal user
        context.Users.Add(new User {  UserName = "admin", UserPassword = "admin123", UserEmail = "admin@example.com" });
        await context.SaveChangesAsync(CancellationToken.None);

        // Simulate SQL injection attempt
        string maliciousInput = "admin' OR '1'='1";

        // Act
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.UserName == maliciousInput, cancellationToken: CancellationToken.None);

        // Assert
        Assert.Null(user); // Should not find a user with injected input
    }
}
