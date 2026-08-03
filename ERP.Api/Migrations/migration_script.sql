IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260730204321_InitialCreate'
)
BEGIN
    CREATE TABLE [Products] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(150) NOT NULL,
        [Description] nvarchar(500) NULL,
        [Price] decimal(18,2) NOT NULL,
        [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_Products] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260730204321_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260730204321_InitialCreate', N'9.0.10');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260730221504_TableUser'
)
BEGIN
    CREATE TABLE [Users] (
        [Id] int NOT NULL IDENTITY,
        [UserName] nvarchar(100) NOT NULL,
        [UserEmail] nvarchar(150) NOT NULL,
        [Password] nvarchar(255) NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260730221504_TableUser'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Users_UserEmail] ON [Users] ([UserEmail]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260730221504_TableUser'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260730221504_TableUser', N'9.0.10');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260731144917_UpdateUserTable'
)
BEGIN
    DROP TABLE [Products];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260731144917_UpdateUserTable'
)
BEGIN
    DROP INDEX [IX_Users_UserEmail] ON [Users];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260731144917_UpdateUserTable'
)
BEGIN
    EXEC sp_rename N'[Users].[Id]', N'ID', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260731144917_UpdateUserTable'
)
BEGIN
    DECLARE @var sysname;
    SELECT @var = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'UserName');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var + '];');
    ALTER TABLE [Users] ALTER COLUMN [UserName] nvarchar(max) NOT NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260731144917_UpdateUserTable'
)
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'UserEmail');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [Users] ALTER COLUMN [UserEmail] nvarchar(max) NOT NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260731144917_UpdateUserTable'
)
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'Password');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [Users] ALTER COLUMN [Password] nvarchar(max) NOT NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260731144917_UpdateUserTable'
)
BEGIN
    ALTER TABLE [Users] ADD [Active] bit NOT NULL DEFAULT CAST(1 AS bit);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260731144917_UpdateUserTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260731144917_UpdateUserTable', N'9.0.10');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260731221143_AddRolesAndPermissions'
)
BEGIN
    ALTER TABLE [Users] ADD [RoleId] int NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260731221143_AddRolesAndPermissions'
)
BEGIN
    CREATE TABLE [Permissions] (
        [ID] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [Description] nvarchar(500) NOT NULL,
        [Module] nvarchar(50) NOT NULL,
        [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
        CONSTRAINT [PK_Permissions] PRIMARY KEY ([ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260731221143_AddRolesAndPermissions'
)
BEGIN
    CREATE TABLE [Roles] (
        [ID] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [Description] nvarchar(500) NOT NULL,
        [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
        CONSTRAINT [PK_Roles] PRIMARY KEY ([ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260731221143_AddRolesAndPermissions'
)
BEGIN
    CREATE TABLE [RolePermissions] (
        [ID] int NOT NULL IDENTITY,
        [RoleId] int NOT NULL,
        [PermissionId] int NOT NULL,
        [Active] bit NOT NULL,
        CONSTRAINT [PK_RolePermissions] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_RolePermissions_Permissions_PermissionId] FOREIGN KEY ([PermissionId]) REFERENCES [Permissions] ([ID]) ON DELETE CASCADE,
        CONSTRAINT [FK_RolePermissions_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([ID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260731221143_AddRolesAndPermissions'
)
BEGIN
    CREATE INDEX [IX_Users_RoleId] ON [Users] ([RoleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260731221143_AddRolesAndPermissions'
)
BEGIN
    CREATE INDEX [IX_RolePermissions_PermissionId] ON [RolePermissions] ([PermissionId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260731221143_AddRolesAndPermissions'
)
BEGIN
    CREATE UNIQUE INDEX [IX_RolePermissions_RoleId_PermissionId] ON [RolePermissions] ([RoleId], [PermissionId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260731221143_AddRolesAndPermissions'
)
BEGIN
    ALTER TABLE [Users] ADD CONSTRAINT [FK_Users_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([ID]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260731221143_AddRolesAndPermissions'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260731221143_AddRolesAndPermissions', N'9.0.10');
END;

COMMIT;
GO

-- Insert default data (roles, permissions)
BEGIN TRANSACTION;

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
    SELECT @AdminRoleId, p.ID, 1 FROM [Permissions] p
    WHERE p.Active = 1
    AND NOT EXISTS (
        SELECT 1 FROM [RolePermissions] rp 
        WHERE rp.RoleId = @AdminRoleId AND rp.PermissionId = p.ID
    );
END;

COMMIT;
GO

PRINT 'Migration completed successfully: Roles, Permissions, and RolePermissions tables created with default data.';
