-- Check if the current user has permission to create databases in SQL Server

IF IS_SRVROLEMEMBER('dbcreator') = 1
BEGIN
    PRINT 'User has permission to create databases.'
    IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'SafeVault')
    BEGIN
        CREATE DATABASE SafeVault;
        PRINT 'Database SafeVault created successfully.'
    END
    ELSE
    BEGIN
        PRINT 'Database SafeVault already exists.'
    END
    
    -- Create a table called Users in the SafeVault database
    USE SafeVault;
    
    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
    BEGIN
        CREATE TABLE Users (
            UserID INT IDENTITY(1,1) PRIMARY KEY,
            UserName NVARCHAR(100),
            UserEmail NVARCHAR(100),
            UserPassword NVARCHAR(100)
        );
        PRINT 'Table Users created successfully in SafeVault database.'
    END
    ELSE
    BEGIN
        PRINT 'Table Users already exists in SafeVault database.'
        DROP TABLE Users;
        CREATE TABLE Users (
            UserID INT IDENTITY(1,1) PRIMARY KEY,
            UserName NVARCHAR(100),
            UserEmail NVARCHAR(100),
            UserPassword NVARCHAR(100)
        );
        PRINT 'Table Users dropped and recreated in SafeVault database.';
    END

    -- Create AccountRole table
    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AccountRole')
    BEGIN
        CREATE TABLE AccountRole (
            AccountRoleID INT IDENTITY(1,1) PRIMARY KEY,
            RoleName NVARCHAR(100) NOT NULL
        );
        PRINT 'Table AccountRole created successfully in SafeVault database.'
    END
    ELSE
    BEGIN
        PRINT 'Table AccountRole already exists in SafeVault database.'
    END

    -- Create UserAccountRole join table
    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserAccountRole')
    BEGIN
        CREATE TABLE UserAccountRole (
            UserAccountRoleID INT IDENTITY(1,1) PRIMARY KEY,
            UserID INT NOT NULL,
            AccountRoleID INT NOT NULL,
            FOREIGN KEY (UserID) REFERENCES Users(UserID),
            FOREIGN KEY (AccountRoleID) REFERENCES AccountRole(AccountRoleID)
        );
        PRINT 'Table UserAccountRole created successfully in SafeVault database.'
    END
    ELSE
    BEGIN
        PRINT 'Table UserAccountRole already exists in SafeVault database.'
    END

END
ELSE
BEGIN
    PRINT 'Permission to create databases is required.'
END