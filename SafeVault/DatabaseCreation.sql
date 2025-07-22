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
                        UserEmail NVARCHAR(100)
        );
        PRINT 'Table Users created successfully in SafeVault database.'
    END
    ELSE
    BEGIN
        PRINT 'Table Users already exists in SafeVault database.'
    END
END
ELSE
BEGIN
    PRINT 'Permission to create databases is required.'
END