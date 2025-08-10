# SafeVault

SafeVault is a secure ASP.NET Core web application implementing role-based access control, user authentication, and strong protection against SQL injection attacks. It leverages Razor Pages, Entity Framework Core, and ASP.NET Core Identity to provide customizable user and role management.

## Features

- **ASP.NET Core Identity Integration**  
  Secure login, registration, password hashing, and token-based authentication.

- **Role-Based Authorization**  
  - `/` – Requires authentication for all Razor Pages by default.  
  - `/UserPages` – Restricted to users with the **Admin** role.  
  - Identity area login, register, and account management pages remain open to anonymous users.

- **Custom Models**  
  - `User` inherits from `IdentityUser<int>` with added properties and default initialization.
  - `AccountRole` supports named roles (`Admin`, `User`, etc.).
  - `UserAccountRole` explicitly defines `UserId` and `RoleId` to avoid property conflicts.

- **Database Integration**  
  Uses **Entity Framework Core** with SQL Server, supporting migrations for schema creation and updates.

- **Security Enhancements**  
  - Custom policies: `RequireAuthenticatedUser` and `RequireAdminRole`.
  - SQL injection prevention validated with automated tests.
  - Default admin account seeded at startup.

- **Testing**  
  Includes `SqlInjectionTests` to ensure protection against injection in username, email, and password lookups.

## Getting Started

### Prerequisites
- [.NET 8.0 or later](https://dotnet.microsoft.com/download)
- SQL Server or LocalDB
- Visual Studio 2022 or Visual Studio Code

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/SafeVault.git
   cd SafeVault

	2.	Configure the database connection
Update appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=SafeVault;User ID=sa;Password=yourpassword;Encrypt=False;"
}


	3.	Apply migrations

dotnet ef database update


	4.	Run the application

dotnet run

Visit: https://localhost:5001

Default Admin Account

A default admin account is seeded automatically:
	•	Email: admin@example.com
	•	Password: Admin@123

Note: Change this password immediately in production.

Authorization Policies

Policy	Description
RequireAuthenticatedUser	Requires the user to be logged in
RequireAdminRole	Requires the user to belong to the Admin role

Project Structure

SafeVault/
├── Controllers/           # MVC controllers
├── Migrations/            # EF Core migrations
├── Models/                # Entity and Identity models
├── Pages/                 # Razor Pages
├── Program.cs             # Application startup
├── appsettings.json       # Configuration
└── SafeVault.csproj       # Project file

Running Tests

dotnet test

License

MIT License. See LICENSE for details.

