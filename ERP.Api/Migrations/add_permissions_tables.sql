-- Migration to add Roles, Permissions, and RolePermissions tables
-- Also adds RoleId column to Users table

BEGIN TRANSACTION;

-- Add RoleId column to Users table
IF NOT EXISTS (
    SELECT * FROM sys.columns 
    WHERE object_id = OBJECT_ID('Users') AND name = 'RoleId'
)
BEGIN
    ALTER TABLE [Users] ADD [RoleId] int NULL;
END;

-- Create Roles table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Roles')
BEGIN
    CREATE TABLE [Roles] (
        [ID] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [Description] nvarchar(500) NULL,
        [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
        CONSTRAINT [PK_Roles] PRIMARY KEY ([ID])
    );
END;

-- Create Permissions table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Permissions')
BEGIN
    CREATE TABLE [Permissions] (
        [ID] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [Description] nvarchar(500) NULL,
        [Module] nvarchar(50) NOT NULL,
        [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
        CONSTRAINT [PK_Permissions] PRIMARY KEY ([ID])
    );
END;

-- Create RolePermissions table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'RolePermissions')
BEGIN
    CREATE TABLE [RolePermissions] (
        [ID] int NOT NULL IDENTITY,
        [RoleId] int NOT NULL,
        [PermissionId] int NOT NULL,
        [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
        CONSTRAINT [PK_RolePermissions] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_RolePermissions_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([ID]) ON DELETE CASCADE,
        CONSTRAINT [FK_RolePermissions_Permissions_PermissionId] FOREIGN KEY ([PermissionId]) REFERENCES [Permissions] ([ID]) ON DELETE CASCADE
    );
END;

-- Create unique index for RolePermissions to prevent duplicate assignments
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_RolePermissions_RoleId_PermissionId' AND object_id = OBJECT_ID('RolePermissions'))
BEGIN
    CREATE UNIQUE INDEX [IX_RolePermissions_RoleId_PermissionId] ON [RolePermissions] ([RoleId], [PermissionId]);
END;

-- Create foreign key constraint for Users.RoleId
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Users_Roles_RoleId')
BEGIN
    ALTER TABLE [Users] ADD CONSTRAINT [FK_Users_Roles_RoleId] 
    FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([ID]) ON DELETE RESTRICT;
END;

-- Insert default roles
IF NOT EXISTS (SELECT * FROM [Roles] WHERE [Name] = 'Administrator')
BEGIN
    INSERT INTO [Roles] ([Name], [Description], [Active]) 
    VALUES ('Administrator', 'Full system access', 1);
END;

IF NOT EXISTS (SELECT * FROM [Roles] WHERE [Name] = 'Manager')
BEGIN
    INSERT INTO [Roles] ([Name], [Description], [Active]) 
    VALUES ('Manager', 'Management level access', 1);
END;

IF NOT EXISTS (SELECT * FROM [Roles] WHERE [Name] = 'Operator')
BEGIN
    INSERT INTO [Roles] ([Name], [Description], [Active]) 
    VALUES ('Operator', 'Basic operational access', 1);
END;

-- Insert default permissions
IF NOT EXISTS (SELECT * FROM [Permissions] WHERE [Name] = 'View Dashboard' AND [Module] = 'Dashboard')
BEGIN
    INSERT INTO [Permissions] ([Name], [Description], [Module], [Active]) 
    VALUES ('View Dashboard', 'Can view the main dashboard', 'Dashboard', 1);
END;

IF NOT EXISTS (SELECT * FROM [Permissions] WHERE [Name] = 'View Production' AND [Module] = 'Production')
BEGIN
    INSERT INTO [Permissions] ([Name], [Description], [Module], [Active]) 
    VALUES ('View Production', 'Can view production module', 'Production', 1);
END;

IF NOT EXISTS (SELECT * FROM [Permissions] WHERE [Name] = 'Create PO' AND [Module] = 'Production')
BEGIN
    INSERT INTO [Permissions] ([Name], [Description], [Module], [Active]) 
    VALUES ('Create PO', 'Can create purchase orders', 'Production', 1);
END;

IF NOT EXISTS (SELECT * FROM [Permissions] WHERE [Name] = 'Edit PO' AND [Module] = 'Production')
BEGIN
    INSERT INTO [Permissions] ([Name], [Description], [Module], [Active]) 
    VALUES ('Edit PO', 'Can edit purchase orders', 'Production', 1);
END;

IF NOT EXISTS (SELECT * FROM [Permissions] WHERE [Name] = 'View Warehouse' AND [Module] = 'Warehouse')
BEGIN
    INSERT INTO [Permissions] ([Name], [Description], [Module], [Active]) 
    VALUES ('View Warehouse', 'Can view warehouse module', 'Warehouse', 1);
END;

IF NOT EXISTS (SELECT * FROM [Permissions] WHERE [Name] = 'Manage Inventory' AND [Module] = 'Warehouse')
BEGIN
    INSERT INTO [Permissions] ([Name], [Description], [Module], [Active]) 
    VALUES ('Manage Inventory', 'Can manage inventory items', 'Warehouse', 1);
END;

IF NOT EXISTS (SELECT * FROM [Permissions] WHERE [Name] = 'View Users' AND [Module] = 'Admin')
BEGIN
    INSERT INTO [Permissions] ([Name], [Description], [Module], [Active]) 
    VALUES ('View Users', 'Can view users list', 'Admin', 1);
END;

IF NOT EXISTS (SELECT * FROM [Permissions] WHERE [Name] = 'Create Users' AND [Module] = 'Admin')
BEGIN
    INSERT INTO [Permissions] ([Name], [Description], [Module], [Active]) 
    VALUES ('Create Users', 'Can create new users', 'Admin', 1);
END;

IF NOT EXISTS (SELECT * FROM [Permissions] WHERE [Name] = 'Edit Users' AND [Module] = 'Admin')
BEGIN
    INSERT INTO [Permissions] ([Name], [Description], [Module], [Active]) 
    VALUES ('Edit Users', 'Can edit user information', 'Admin', 1);
END;

IF NOT EXISTS (SELECT * FROM [Permissions] WHERE [Name] = 'View Roles' AND [Module] = 'Admin')
BEGIN
    INSERT INTO [Permissions] ([Name], [Description], [Module], [Active]) 
    VALUES ('View Roles', 'Can view roles list', 'Admin', 1);
END;

IF NOT EXISTS (SELECT * FROM [Permissions] WHERE [Name] = 'Manage Roles' AND [Module] = 'Admin')
BEGIN
    INSERT INTO [Permissions] ([Name], [Description], [Module], [Active]) 
    VALUES ('Manage Roles', 'Can create and edit roles', 'Admin', 1);
END;

IF NOT EXISTS (SELECT * FROM [Permissions] WHERE [Name] = 'View Clients' AND [Module] = 'Admin')
BEGIN
    INSERT INTO [Permissions] ([Name], [Description], [Module], [Active]) 
    VALUES ('View Clients', 'Can view clients list', 'Admin', 1);
END;

IF NOT EXISTS (SELECT * FROM [Permissions] WHERE [Name] = 'Manage Clients' AND [Module] = 'Admin')
BEGIN
    INSERT INTO [Permissions] ([Name], [Description], [Module], [Active]) 
    VALUES ('Manage Clients', 'Can create and edit clients', 'Admin', 1);
END;

-- Assign all permissions to Administrator role
DECLARE @AdminRoleId int;
SELECT @AdminRoleId = ID FROM [Roles] WHERE [Name] = 'Administrator';

IF @AdminRoleId IS NOT NULL
BEGIN
    INSERT INTO [RolePermissions] ([RoleId], [PermissionId], [Active])
    SELECT @AdminRoleId, ID, 1 FROM [Permissions] WHERE [Active] = 1
    WHERE NOT EXISTS (
        SELECT 1 FROM [RolePermissions] 
        WHERE RoleId = @AdminRoleId AND PermissionId = [Permissions].ID
    );
END;

COMMIT;
GO

PRINT 'Migration completed successfully: Roles, Permissions, and RolePermissions tables created with default data.';
