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
CREATE TABLE [Products] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(150) NOT NULL,
    [Description] nvarchar(500) NULL,
    [Price] decimal(18,2) NOT NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([Id])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260730204321_InitialCreate', N'9.0.10');

CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [UserName] nvarchar(100) NOT NULL,
    [UserEmail] nvarchar(150) NOT NULL,
    [Password] nvarchar(255) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);

CREATE UNIQUE INDEX [IX_Users_UserEmail] ON [Users] ([UserEmail]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260730221504_TableUser', N'9.0.10');

DROP TABLE [Products];

DROP INDEX [IX_Users_UserEmail] ON [Users];

EXEC sp_rename N'[Users].[Id]', N'ID', 'COLUMN';

DECLARE @var sysname;
SELECT @var = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'UserName');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var + '];');
ALTER TABLE [Users] ALTER COLUMN [UserName] nvarchar(max) NOT NULL;

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'UserEmail');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Users] ALTER COLUMN [UserEmail] nvarchar(max) NOT NULL;

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'Password');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Users] ALTER COLUMN [Password] nvarchar(max) NOT NULL;

ALTER TABLE [Users] ADD [Active] bit NOT NULL DEFAULT CAST(1 AS bit);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260731144917_UpdateUserTable', N'9.0.10');

ALTER TABLE [Users] ADD [RoleId] int NULL;

CREATE TABLE [Permissions] (
    [ID] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Description] nvarchar(500) NOT NULL,
    [Module] nvarchar(50) NOT NULL,
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    CONSTRAINT [PK_Permissions] PRIMARY KEY ([ID])
);

CREATE TABLE [Roles] (
    [ID] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Description] nvarchar(500) NOT NULL,
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    CONSTRAINT [PK_Roles] PRIMARY KEY ([ID])
);

CREATE TABLE [RolePermissions] (
    [ID] int NOT NULL IDENTITY,
    [RoleId] int NOT NULL,
    [PermissionId] int NOT NULL,
    [Active] bit NOT NULL,
    CONSTRAINT [PK_RolePermissions] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_RolePermissions_Permissions_PermissionId] FOREIGN KEY ([PermissionId]) REFERENCES [Permissions] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_RolePermissions_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([ID]) ON DELETE CASCADE
);

CREATE INDEX [IX_Users_RoleId] ON [Users] ([RoleId]);

CREATE INDEX [IX_RolePermissions_PermissionId] ON [RolePermissions] ([PermissionId]);

CREATE UNIQUE INDEX [IX_RolePermissions_RoleId_PermissionId] ON [RolePermissions] ([RoleId], [PermissionId]);

ALTER TABLE [Users] ADD CONSTRAINT [FK_Users_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([ID]) ON DELETE NO ACTION;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260731221143_AddRolesAndPermissions', N'9.0.10');

ALTER TABLE [Users] ADD [MustChangePassword] bit NOT NULL DEFAULT CAST(1 AS bit);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260802002213_AddMustChangePasswordToUser', N'9.0.10');

CREATE TABLE [Fgpos] (
    [ID] int NOT NULL IDENTITY,
    [FGPONumber] nvarchar(50) NOT NULL,
    [TemporaryNumber] nvarchar(50) NULL,
    [Customer] nvarchar(100) NOT NULL,
    [Buyer] nvarchar(100) NULL,
    [Brand] nvarchar(100) NULL,
    [Style] nvarchar(100) NULL,
    [StyleDescription] nvarchar(500) NULL,
    [Season] nvarchar(50) NULL,
    [PurchaseOrder] nvarchar(100) NULL,
    [Color] nvarchar(50) NULL,
    [SizeRange] nvarchar(50) NULL,
    [OrderQuantity] int NOT NULL,
    [DeliveryDate] datetime2 NOT NULL,
    [Factory] nvarchar(100) NOT NULL,
    [Status] nvarchar(50) NULL,
    [Remarks] nvarchar(1000) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    CONSTRAINT [PK_Fgpos] PRIMARY KEY ([ID])
);

CREATE UNIQUE INDEX [IX_Fgpos_FGPONumber] ON [Fgpos] ([FGPONumber]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260803155118_AddFgpoTable', N'9.0.10');

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Fgpos]') AND [c].[name] = N'Customer');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Fgpos] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Fgpos] DROP COLUMN [Customer];

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Fgpos]') AND [c].[name] = N'Factory');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Fgpos] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [Fgpos] DROP COLUMN [Factory];

ALTER TABLE [Fgpos] ADD [CustomerId] int NOT NULL DEFAULT 0;

ALTER TABLE [Fgpos] ADD [FactoryId] int NOT NULL DEFAULT 0;

CREATE TABLE [Customers] (
    [ID] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Contact] nvarchar(100) NULL,
    [Phone] nvarchar(50) NULL,
    [Email] nvarchar(100) NULL,
    [Address] nvarchar(200) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    CONSTRAINT [PK_Customers] PRIMARY KEY ([ID])
);

CREATE TABLE [Factories] (
    [ID] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Location] nvarchar(200) NULL,
    [Contact] nvarchar(100) NULL,
    [Phone] nvarchar(50) NULL,
    [Email] nvarchar(100) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    CONSTRAINT [PK_Factories] PRIMARY KEY ([ID])
);

CREATE INDEX [IX_Fgpos_CustomerId] ON [Fgpos] ([CustomerId]);

CREATE INDEX [IX_Fgpos_FactoryId] ON [Fgpos] ([FactoryId]);

CREATE UNIQUE INDEX [IX_Customers_Name] ON [Customers] ([Name]);

CREATE UNIQUE INDEX [IX_Factories_Name] ON [Factories] ([Name]);

ALTER TABLE [Fgpos] ADD CONSTRAINT [FK_Fgpos_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([ID]) ON DELETE NO ACTION;

ALTER TABLE [Fgpos] ADD CONSTRAINT [FK_Fgpos_Factories_FactoryId] FOREIGN KEY ([FactoryId]) REFERENCES [Factories] ([ID]) ON DELETE NO ACTION;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260803173344_AddCustomersAndFactories', N'9.0.10');

CREATE TABLE [FabricRequirements] (
    [ID] int NOT NULL IDENTITY,
    [FGPOId] int NOT NULL,
    [LineNumber] int NOT NULL,
    [FabricCode] nvarchar(50) NOT NULL,
    [FabricName] nvarchar(100) NULL,
    [FabricType] nvarchar(50) NULL,
    [Color] nvarchar(50) NULL,
    [ColorCode] nvarchar(50) NULL,
    [Supplier] nvarchar(100) NULL,
    [Composition] nvarchar(200) NULL,
    [Width] nvarchar(50) NULL,
    [Weight] nvarchar(50) NULL,
    [UnitOfMeasure] nvarchar(20) NULL,
    [ConsumptionPerGarment] decimal(18,4) NOT NULL,
    [WastePercentage] decimal(18,4) NOT NULL,
    [NetRequirement] decimal(18,4) NOT NULL,
    [GrossRequirement] decimal(18,4) NOT NULL,
    [OrderedQuantity] decimal(18,4) NOT NULL,
    [ReceivedQuantity] decimal(18,4) NOT NULL,
    [ReservedQuantity] decimal(18,4) NOT NULL,
    [IssuedQuantity] decimal(18,4) NOT NULL,
    [BalanceQuantity] decimal(18,4) NOT NULL,
    [Remarks] nvarchar(1000) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    CONSTRAINT [PK_FabricRequirements] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_FabricRequirements_Fgpos_FGPOId] FOREIGN KEY ([FGPOId]) REFERENCES [Fgpos] ([ID]) ON DELETE NO ACTION
);

CREATE INDEX [IX_FabricRequirements_FGPOId] ON [FabricRequirements] ([FGPOId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260803195928_AddFabricRequirementTable', N'9.0.10');

ALTER TABLE [Fgpos] DROP CONSTRAINT [FK_Fgpos_Factories_FactoryId];

DROP INDEX [IX_Fgpos_FactoryId] ON [Fgpos];

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Fgpos]') AND [c].[name] = N'Brand');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Fgpos] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [Fgpos] DROP COLUMN [Brand];

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Fgpos]') AND [c].[name] = N'Buyer');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Fgpos] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [Fgpos] DROP COLUMN [Buyer];

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Fgpos]') AND [c].[name] = N'FactoryId');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Fgpos] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [Fgpos] DROP COLUMN [FactoryId];

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Fgpos]') AND [c].[name] = N'Season');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [Fgpos] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [Fgpos] DROP COLUMN [Season];

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Fgpos]') AND [c].[name] = N'SizeRange');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [Fgpos] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [Fgpos] DROP COLUMN [SizeRange];

DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Fgpos]') AND [c].[name] = N'StyleDescription');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [Fgpos] DROP CONSTRAINT [' + @var10 + '];');
ALTER TABLE [Fgpos] DROP COLUMN [StyleDescription];

DECLARE @var11 sysname;
SELECT @var11 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[FabricRequirements]') AND [c].[name] = N'BalanceQuantity');
IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [FabricRequirements] DROP CONSTRAINT [' + @var11 + '];');
ALTER TABLE [FabricRequirements] DROP COLUMN [BalanceQuantity];

DECLARE @var12 sysname;
SELECT @var12 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[FabricRequirements]') AND [c].[name] = N'ColorCode');
IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [FabricRequirements] DROP CONSTRAINT [' + @var12 + '];');
ALTER TABLE [FabricRequirements] DROP COLUMN [ColorCode];

DECLARE @var13 sysname;
SELECT @var13 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[FabricRequirements]') AND [c].[name] = N'FabricCode');
IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [FabricRequirements] DROP CONSTRAINT [' + @var13 + '];');
ALTER TABLE [FabricRequirements] DROP COLUMN [FabricCode];

DECLARE @var14 sysname;
SELECT @var14 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[FabricRequirements]') AND [c].[name] = N'LineNumber');
IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [FabricRequirements] DROP CONSTRAINT [' + @var14 + '];');
ALTER TABLE [FabricRequirements] DROP COLUMN [LineNumber];

EXEC sp_rename N'[Fgpos].[PurchaseOrder]', N'DataOwner', 'COLUMN';

EXEC sp_rename N'[FabricRequirements].[Width]', N'Status', 'COLUMN';

EXEC sp_rename N'[FabricRequirements].[Weight]', N'RequiredWidth', 'COLUMN';

EXEC sp_rename N'[FabricRequirements].[WastePercentage]', N'OrderQuantity', 'COLUMN';

EXEC sp_rename N'[FabricRequirements].[UnitOfMeasure]', N'UOM', 'COLUMN';

EXEC sp_rename N'[FabricRequirements].[Supplier]', N'Style', 'COLUMN';

EXEC sp_rename N'[FabricRequirements].[ReservedQuantity]', N'NetPurchaseRequirement', 'COLUMN';

EXEC sp_rename N'[FabricRequirements].[ReceivedQuantity]', N'GSM', 'COLUMN';

EXEC sp_rename N'[FabricRequirements].[OrderedQuantity]', N'AvailableInventory', 'COLUMN';

EXEC sp_rename N'[FabricRequirements].[NetRequirement]', N'ApprovedYield', 'COLUMN';

EXEC sp_rename N'[FabricRequirements].[IssuedQuantity]', N'AllowanceQty', 'COLUMN';

EXEC sp_rename N'[FabricRequirements].[FabricType]', N'FabricComponent', 'COLUMN';

EXEC sp_rename N'[FabricRequirements].[FabricName]', N'DataOwner', 'COLUMN';

EXEC sp_rename N'[FabricRequirements].[ConsumptionPerGarment]', N'AllowancePercentage', 'COLUMN';

ALTER TABLE [Fgpos] ADD [InTransitQty] decimal(18,4) NOT NULL DEFAULT 0.0;

ALTER TABLE [Fgpos] ADD [OverproductionQty] decimal(18,4) NOT NULL DEFAULT 0.0;

ALTER TABLE [Fgpos] ADD [OvershipmentQty] decimal(18,4) NOT NULL DEFAULT 0.0;

ALTER TABLE [Fgpos] ADD [PendingProduction] decimal(18,4) NOT NULL DEFAULT 0.0;

ALTER TABLE [Fgpos] ADD [PendingToShip] decimal(18,4) NOT NULL DEFAULT 0.0;

ALTER TABLE [Fgpos] ADD [ProducedQty] decimal(18,4) NOT NULL DEFAULT 0.0;

ALTER TABLE [Fgpos] ADD [ProductionVariance] decimal(18,4) NOT NULL DEFAULT 0.0;

ALTER TABLE [Fgpos] ADD [ReceivedQty] decimal(18,4) NOT NULL DEFAULT 0.0;

ALTER TABLE [Fgpos] ADD [ShipmentVariance] decimal(18,4) NOT NULL DEFAULT 0.0;

ALTER TABLE [Fgpos] ADD [TotalShippedQty] decimal(18,4) NOT NULL DEFAULT 0.0;

ALTER TABLE [FabricRequirements] ADD [FabricDescription] nvarchar(200) NULL;

ALTER TABLE [FabricRequirements] ADD [RequiredDate] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260803204448_FgpoMasterSpecUpdate', N'9.0.10');

CREATE TABLE [FabricPOs] (
    [ID] int NOT NULL IDENTITY,
    [FabricPONumber] nvarchar(50) NOT NULL,
    [Supplier] nvarchar(100) NULL,
    [FabricMill] nvarchar(100) NULL,
    [FabricComponent] nvarchar(50) NULL,
    [Style] nvarchar(100) NULL,
    [Color] nvarchar(50) NULL,
    [OrderedQuantity] decimal(18,4) NOT NULL,
    [UOM] nvarchar(20) NULL,
    [UnitPrice] decimal(18,4) NOT NULL,
    [POAmount] decimal(18,4) NOT NULL,
    [OrderDate] datetime2 NOT NULL,
    [RequiredCompletion] datetime2 NOT NULL,
    [PlannedExport] datetime2 NULL,
    [PlannedArrival] datetime2 NULL,
    [POStatus] nvarchar(50) NULL,
    [PurchaseOwner] nvarchar(100) NULL,
    [ApprovedBy] nvarchar(100) NULL,
    [LastUpdated] datetime2 NULL,
    [Remarks] nvarchar(1000) NULL,
    [DataOwner] nvarchar(100) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    CONSTRAINT [PK_FabricPOs] PRIMARY KEY ([ID])
);

CREATE TABLE [FabricPOFgpos] (
    [FabricPOId] int NOT NULL,
    [FGPOId] int NOT NULL,
    CONSTRAINT [PK_FabricPOFgpos] PRIMARY KEY ([FabricPOId], [FGPOId]),
    CONSTRAINT [FK_FabricPOFgpos_FabricPOs_FabricPOId] FOREIGN KEY ([FabricPOId]) REFERENCES [FabricPOs] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_FabricPOFgpos_Fgpos_FGPOId] FOREIGN KEY ([FGPOId]) REFERENCES [Fgpos] ([ID]) ON DELETE NO ACTION
);

CREATE INDEX [IX_FabricPOFgpos_FGPOId] ON [FabricPOFgpos] ([FGPOId]);

CREATE UNIQUE INDEX [IX_FabricPOs_FabricPONumber] ON [FabricPOs] ([FabricPONumber]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260803221341_AddFabricPO', N'9.0.10');

DECLARE @var15 sysname;
SELECT @var15 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[FabricPOs]') AND [c].[name] = N'Color');
IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [FabricPOs] DROP CONSTRAINT [' + @var15 + '];');
ALTER TABLE [FabricPOs] DROP COLUMN [Color];

DECLARE @var16 sysname;
SELECT @var16 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[FabricPOs]') AND [c].[name] = N'Style');
IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [FabricPOs] DROP CONSTRAINT [' + @var16 + '];');
ALTER TABLE [FabricPOs] DROP COLUMN [Style];

ALTER TABLE [FabricPOFgpos] ADD [AllocatedQuantity] decimal(18,4) NOT NULL DEFAULT 0.0;

ALTER TABLE [FabricPOFgpos] ADD [Color] nvarchar(50) NULL;

ALTER TABLE [FabricPOFgpos] ADD [LastUpdated] datetime2 NULL;

ALTER TABLE [FabricPOFgpos] ADD [Style] nvarchar(100) NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260803225707_AddFabricPOFgpoAllocatedQuantity', N'9.0.10');

ALTER TABLE [FabricPOFgpos] DROP CONSTRAINT [PK_FabricPOFgpos];

DECLARE @var17 sysname;
SELECT @var17 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[FabricPOs]') AND [c].[name] = N'DataOwner');
IF @var17 IS NOT NULL EXEC(N'ALTER TABLE [FabricPOs] DROP CONSTRAINT [' + @var17 + '];');
ALTER TABLE [FabricPOs] DROP COLUMN [DataOwner];

ALTER TABLE [FabricPOFgpos] ADD [ID] int NOT NULL IDENTITY;

ALTER TABLE [FabricPOFgpos] ADD CONSTRAINT [PK_FabricPOFgpos] PRIMARY KEY ([ID]);

CREATE UNIQUE INDEX [IX_FabricPOFgpos_FabricPOId_FGPOId] ON [FabricPOFgpos] ([FabricPOId], [FGPOId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260804134823_FabricPOFgpoEntityRefactor', N'9.0.10');

CREATE TABLE [MillProductions] (
    [ID] int NOT NULL IDENTITY,
    [FabricPOId] int NOT NULL,
    [FGPOId] int NOT NULL,
    [Supplier] nvarchar(100) NULL,
    [FabricComponent] nvarchar(50) NULL,
    [Style] nvarchar(100) NULL,
    [Color] nvarchar(50) NULL,
    [PlannedQuantity] decimal(18,4) NOT NULL,
    [ProducedQuantity] decimal(18,4) NOT NULL,
    [CompletionPercentage] decimal(18,4) NOT NULL,
    [LotNumber] nvarchar(50) NULL,
    [RollQuantity] decimal(18,4) NOT NULL,
    [YardageOrQty] decimal(18,4) NOT NULL,
    [Weight] decimal(18,4) NOT NULL,
    [StartDate] datetime2 NOT NULL,
    [FinishDate] datetime2 NULL,
    [PlannedExport] datetime2 NULL,
    [ActualExport] datetime2 NULL,
    [Status] nvarchar(50) NULL,
    [DataOwner] nvarchar(100) NULL,
    [Remarks] nvarchar(1000) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    CONSTRAINT [PK_MillProductions] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_MillProductions_FabricPOs_FabricPOId] FOREIGN KEY ([FabricPOId]) REFERENCES [FabricPOs] ([ID]) ON DELETE NO ACTION,
    CONSTRAINT [FK_MillProductions_Fgpos_FGPOId] FOREIGN KEY ([FGPOId]) REFERENCES [Fgpos] ([ID]) ON DELETE NO ACTION
);

CREATE TABLE [MillTests] (
    [ID] int NOT NULL IDENTITY,
    [FabricPOId] int NOT NULL,
    [FGPOId] int NOT NULL,
    [Supplier] nvarchar(100) NULL,
    [LotNumber] nvarchar(50) NULL,
    [Color] nvarchar(50) NULL,
    [RollQty] decimal(18,4) NOT NULL,
    [ActualWidth] decimal(18,4) NOT NULL,
    [ActualGSM] decimal(18,4) NOT NULL,
    [LengthShrinkagePercentage] decimal(18,4) NOT NULL,
    [WidthShrinkagePercentage] decimal(18,4) NOT NULL,
    [TorquePercentage] decimal(18,4) NOT NULL,
    [BowingPercentage] decimal(18,4) NOT NULL,
    [SkewingPercentage] decimal(18,4) NOT NULL,
    [Colorfastness] nvarchar(100) NULL,
    [WashAppearance] nvarchar(100) NULL,
    [HandFeel] nvarchar(100) NULL,
    [TestDate] datetime2 NOT NULL,
    [TestedBy] nvarchar(100) NULL,
    [TestResult] nvarchar(50) NULL,
    [ApprovedForExport] bit NOT NULL,
    [ReportLink] nvarchar(500) NULL,
    [Comments] nvarchar(1000) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    CONSTRAINT [PK_MillTests] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_MillTests_FabricPOs_FabricPOId] FOREIGN KEY ([FabricPOId]) REFERENCES [FabricPOs] ([ID]) ON DELETE NO ACTION,
    CONSTRAINT [FK_MillTests_Fgpos_FGPOId] FOREIGN KEY ([FGPOId]) REFERENCES [Fgpos] ([ID]) ON DELETE NO ACTION
);

CREATE INDEX [IX_MillProductions_FabricPOId] ON [MillProductions] ([FabricPOId]);

CREATE INDEX [IX_MillProductions_FGPOId] ON [MillProductions] ([FGPOId]);

CREATE INDEX [IX_MillTests_FabricPOId] ON [MillTests] ([FabricPOId]);

CREATE INDEX [IX_MillTests_FGPOId] ON [MillTests] ([FGPOId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260804162549_AddMillProductionAndMillTest', N'9.0.10');

CREATE TABLE [FabricShipments] (
    [ID] int NOT NULL IDENTITY,
    [ShipmentNumber] nvarchar(50) NOT NULL,
    [FabricPOId] int NOT NULL,
    [FGPOId] int NOT NULL,
    [Supplier] nvarchar(100) NULL,
    [LotNumber] nvarchar(50) NULL,
    [RollQty] decimal(18,4) NOT NULL,
    [ShippedQuantity] decimal(18,4) NOT NULL,
    [UOM] nvarchar(20) NULL,
    [ShippedWeight] decimal(18,4) NOT NULL,
    [PackingList] nvarchar(200) NULL,
    [InvoiceNumber] nvarchar(100) NULL,
    [ContainerAWB] nvarchar(100) NULL,
    [ShippingMethod] nvarchar(100) NULL,
    [ETD] datetime2 NOT NULL,
    [ETA] datetime2 NOT NULL,
    [ShipmentStatus] nvarchar(50) NULL,
    [DeliveredToTexnicaDate] datetime2 NULL,
    [InTransitQuantity] decimal(18,4) NOT NULL,
    [RemainingToDeliver] decimal(18,4) NOT NULL,
    [DataOwner] nvarchar(100) NULL,
    [Remarks] nvarchar(1000) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    CONSTRAINT [PK_FabricShipments] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_FabricShipments_FabricPOs_FabricPOId] FOREIGN KEY ([FabricPOId]) REFERENCES [FabricPOs] ([ID]) ON DELETE NO ACTION,
    CONSTRAINT [FK_FabricShipments_Fgpos_FGPOId] FOREIGN KEY ([FGPOId]) REFERENCES [Fgpos] ([ID]) ON DELETE NO ACTION
);

CREATE INDEX [IX_FabricShipments_FabricPOId] ON [FabricShipments] ([FabricPOId]);

CREATE INDEX [IX_FabricShipments_FGPOId] ON [FabricShipments] ([FGPOId]);

CREATE UNIQUE INDEX [IX_FabricShipments_ShipmentNumber] ON [FabricShipments] ([ShipmentNumber]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260804185049_AddFabricShipment', N'9.0.10');

DROP INDEX [IX_MillTests_FGPOId] ON [MillTests];

DROP INDEX [IX_MillProductions_FGPOId] ON [MillProductions];

DROP INDEX [IX_FabricShipments_FGPOId] ON [FabricShipments];

ALTER TABLE [Users] ADD [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE());

ALTER TABLE [Users] ADD [UpdatedAt] datetime2 NULL;

ALTER TABLE [Roles] ADD [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE());

ALTER TABLE [Roles] ADD [UpdatedAt] datetime2 NULL;

ALTER TABLE [RolePermissions] ADD [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE());

ALTER TABLE [RolePermissions] ADD [UpdatedAt] datetime2 NULL;

ALTER TABLE [Permissions] ADD [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE());

ALTER TABLE [Permissions] ADD [UpdatedAt] datetime2 NULL;

DECLARE @var18 sysname;
SELECT @var18 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MillTests]') AND [c].[name] = N'CreatedAt');
IF @var18 IS NOT NULL EXEC(N'ALTER TABLE [MillTests] DROP CONSTRAINT [' + @var18 + '];');
ALTER TABLE [MillTests] ADD DEFAULT (GETUTCDATE()) FOR [CreatedAt];

ALTER TABLE [MillTests] ADD [LotId] int NULL;

DECLARE @var19 sysname;
SELECT @var19 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MillProductions]') AND [c].[name] = N'CreatedAt');
IF @var19 IS NOT NULL EXEC(N'ALTER TABLE [MillProductions] DROP CONSTRAINT [' + @var19 + '];');
ALTER TABLE [MillProductions] ADD DEFAULT (GETUTCDATE()) FOR [CreatedAt];

ALTER TABLE [MillProductions] ADD [LotId] int NULL;

DECLARE @var20 sysname;
SELECT @var20 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Fgpos]') AND [c].[name] = N'CreatedAt');
IF @var20 IS NOT NULL EXEC(N'ALTER TABLE [Fgpos] DROP CONSTRAINT [' + @var20 + '];');
ALTER TABLE [Fgpos] ADD DEFAULT (GETUTCDATE()) FOR [CreatedAt];

DECLARE @var21 sysname;
SELECT @var21 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Factories]') AND [c].[name] = N'CreatedAt');
IF @var21 IS NOT NULL EXEC(N'ALTER TABLE [Factories] DROP CONSTRAINT [' + @var21 + '];');
ALTER TABLE [Factories] ADD DEFAULT (GETUTCDATE()) FOR [CreatedAt];

DECLARE @var22 sysname;
SELECT @var22 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[FabricShipments]') AND [c].[name] = N'CreatedAt');
IF @var22 IS NOT NULL EXEC(N'ALTER TABLE [FabricShipments] DROP CONSTRAINT [' + @var22 + '];');
ALTER TABLE [FabricShipments] ADD DEFAULT (GETUTCDATE()) FOR [CreatedAt];

ALTER TABLE [FabricShipments] ADD [LotId] int NULL;

DECLARE @var23 sysname;
SELECT @var23 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[FabricRequirements]') AND [c].[name] = N'CreatedAt');
IF @var23 IS NOT NULL EXEC(N'ALTER TABLE [FabricRequirements] DROP CONSTRAINT [' + @var23 + '];');
ALTER TABLE [FabricRequirements] ADD DEFAULT (GETUTCDATE()) FOR [CreatedAt];

DECLARE @var24 sysname;
SELECT @var24 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[FabricPOs]') AND [c].[name] = N'CreatedAt');
IF @var24 IS NOT NULL EXEC(N'ALTER TABLE [FabricPOs] DROP CONSTRAINT [' + @var24 + '];');
ALTER TABLE [FabricPOs] ADD DEFAULT (GETUTCDATE()) FOR [CreatedAt];

DECLARE @var25 sysname;
SELECT @var25 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Customers]') AND [c].[name] = N'CreatedAt');
IF @var25 IS NOT NULL EXEC(N'ALTER TABLE [Customers] DROP CONSTRAINT [' + @var25 + '];');
ALTER TABLE [Customers] ADD DEFAULT (GETUTCDATE()) FOR [CreatedAt];

DECLARE @var26 sysname;
SELECT @var26 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MillProductions]') AND [c].[name] = N'CompletionPercentage');
IF @var26 IS NOT NULL EXEC(N'ALTER TABLE [MillProductions] DROP CONSTRAINT [' + @var26 + '];');
ALTER TABLE [MillProductions] DROP COLUMN [CompletionPercentage];
ALTER TABLE [MillProductions] ADD [CompletionPercentage] AS CAST((CASE WHEN [PlannedQuantity] = 0 THEN 0 ELSE ([ProducedQuantity] / [PlannedQuantity]) * 100 END) AS decimal(18,4));

DECLARE @var27 sysname;
SELECT @var27 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[FabricShipments]') AND [c].[name] = N'RemainingToDeliver');
IF @var27 IS NOT NULL EXEC(N'ALTER TABLE [FabricShipments] DROP CONSTRAINT [' + @var27 + '];');
ALTER TABLE [FabricShipments] DROP COLUMN [RemainingToDeliver];
ALTER TABLE [FabricShipments] ADD [RemainingToDeliver] AS CAST((CASE WHEN [DeliveredToTexnicaDate] IS NULL THEN [ShippedQuantity] ELSE 0 END) AS decimal(18,4));

DECLARE @var28 sysname;
SELECT @var28 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[FabricShipments]') AND [c].[name] = N'InTransitQuantity');
IF @var28 IS NOT NULL EXEC(N'ALTER TABLE [FabricShipments] DROP CONSTRAINT [' + @var28 + '];');
ALTER TABLE [FabricShipments] DROP COLUMN [InTransitQuantity];
ALTER TABLE [FabricShipments] ADD [InTransitQuantity] AS CAST((CASE WHEN [DeliveredToTexnicaDate] IS NULL THEN [ShippedQuantity] ELSE 0 END) AS decimal(18,4));

CREATE TABLE [CatalogValues] (
    [ID] int NOT NULL IDENTITY,
    [Type] nvarchar(50) NOT NULL,
    [Value] nvarchar(100) NOT NULL,
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_CatalogValues] PRIMARY KEY ([ID])
);

CREATE TABLE [Lots] (
    [ID] int NOT NULL IDENTITY,
    [LotNumber] nvarchar(50) NOT NULL,
    [FabricPOId] int NOT NULL,
    [FGPOId] int NOT NULL,
    [ProducedQuantity] decimal(18,4) NOT NULL,
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_Lots] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_Lots_FabricPOs_FabricPOId] FOREIGN KEY ([FabricPOId]) REFERENCES [FabricPOs] ([ID]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Lots_Fgpos_FGPOId] FOREIGN KEY ([FGPOId]) REFERENCES [Fgpos] ([ID]) ON DELETE NO ACTION
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ID', N'Active', N'Type', N'UpdatedAt', N'Value') AND [object_id] = OBJECT_ID(N'[CatalogValues]'))
    SET IDENTITY_INSERT [CatalogValues] ON;
INSERT INTO [CatalogValues] ([ID], [Active], [Type], [UpdatedAt], [Value])
VALUES (1, CAST(1 AS bit), N'UOM', NULL, N'Yards'),
(2, CAST(1 AS bit), N'UOM', NULL, N'Meters'),
(3, CAST(1 AS bit), N'UOM', NULL, N'Kilograms'),
(4, CAST(1 AS bit), N'UOM', NULL, N'Pounds'),
(5, CAST(1 AS bit), N'UOM', NULL, N'Rolls'),
(6, CAST(1 AS bit), N'UOM', NULL, N'Pieces'),
(7, CAST(1 AS bit), N'FabricComponent', NULL, N'Body Fabric'),
(8, CAST(1 AS bit), N'FabricComponent', NULL, N'Rib'),
(9, CAST(1 AS bit), N'FabricComponent', NULL, N'Shoulder Tape'),
(10, CAST(1 AS bit), N'FabricComponent', NULL, N'Neck Tape'),
(11, CAST(1 AS bit), N'FabricComponent', NULL, N'Pocketing'),
(12, CAST(1 AS bit), N'FabricComponent', NULL, N'Other'),
(13, CAST(1 AS bit), N'ProductionStatus', NULL, N'Not Started'),
(14, CAST(1 AS bit), N'ProductionStatus', NULL, N'Pending'),
(15, CAST(1 AS bit), N'ProductionStatus', NULL, N'In Progress'),
(16, CAST(1 AS bit), N'ProductionStatus', NULL, N'Partially Completed'),
(17, CAST(1 AS bit), N'ProductionStatus', NULL, N'Completed'),
(18, CAST(1 AS bit), N'ProductionStatus', NULL, N'On Hold'),
(19, CAST(1 AS bit), N'ProductionStatus', NULL, N'Cancelled'),
(20, CAST(1 AS bit), N'TestResult', NULL, N'Pending'),
(21, CAST(1 AS bit), N'TestResult', NULL, N'Testing'),
(22, CAST(1 AS bit), N'TestResult', NULL, N'Passed'),
(23, CAST(1 AS bit), N'TestResult', NULL, N'Conditionally Passed'),
(24, CAST(1 AS bit), N'TestResult', NULL, N'Failed'),
(25, CAST(1 AS bit), N'ShipmentStatus', NULL, N'Planned'),
(26, CAST(1 AS bit), N'ShipmentStatus', NULL, N'Booking Confirmed'),
(27, CAST(1 AS bit), N'ShipmentStatus', NULL, N'Exported'),
(28, CAST(1 AS bit), N'ShipmentStatus', NULL, N'In Transit'),
(29, CAST(1 AS bit), N'ShipmentStatus', NULL, N'Delivered'),
(30, CAST(1 AS bit), N'ShipmentStatus', NULL, N'Cancelled'),
(31, CAST(1 AS bit), N'POStatus', NULL, N'Not Started'),
(32, CAST(1 AS bit), N'POStatus', NULL, N'Pending'),
(33, CAST(1 AS bit), N'POStatus', NULL, N'In Progress'),
(34, CAST(1 AS bit), N'POStatus', NULL, N'Partially Completed'),
(35, CAST(1 AS bit), N'POStatus', NULL, N'Completed'),
(36, CAST(1 AS bit), N'POStatus', NULL, N'Approved'),
(37, CAST(1 AS bit), N'POStatus', NULL, N'Conditionally Approved'),
(38, CAST(1 AS bit), N'POStatus', NULL, N'Rejected'),
(39, CAST(1 AS bit), N'POStatus', NULL, N'On Hold'),
(40, CAST(1 AS bit), N'POStatus', NULL, N'Closed'),
(41, CAST(1 AS bit), N'POStatus', NULL, N'Cancelled');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ID', N'Active', N'Type', N'UpdatedAt', N'Value') AND [object_id] = OBJECT_ID(N'[CatalogValues]'))
    SET IDENTITY_INSERT [CatalogValues] OFF;

CREATE INDEX [IX_MillTests_FGPOId_FabricPOId] ON [MillTests] ([FGPOId], [FabricPOId]);

CREATE INDEX [IX_MillTests_LotId] ON [MillTests] ([LotId]);

CREATE INDEX [IX_MillTests_LotNumber] ON [MillTests] ([LotNumber]);

CREATE INDEX [IX_MillTests_TestResult] ON [MillTests] ([TestResult]);

CREATE INDEX [IX_MillProductions_FGPOId_FabricPOId] ON [MillProductions] ([FGPOId], [FabricPOId]);

CREATE INDEX [IX_MillProductions_LotId] ON [MillProductions] ([LotId]);

CREATE INDEX [IX_MillProductions_LotNumber] ON [MillProductions] ([LotNumber]);

CREATE INDEX [IX_MillProductions_Status] ON [MillProductions] ([Status]);

CREATE INDEX [IX_FabricShipments_FGPOId_FabricPOId] ON [FabricShipments] ([FGPOId], [FabricPOId]);

CREATE INDEX [IX_FabricShipments_LotId] ON [FabricShipments] ([LotId]);

CREATE INDEX [IX_FabricShipments_LotNumber] ON [FabricShipments] ([LotNumber]);

CREATE INDEX [IX_FabricShipments_ShipmentStatus] ON [FabricShipments] ([ShipmentStatus]);

CREATE UNIQUE INDEX [IX_CatalogValues_Type_Value] ON [CatalogValues] ([Type], [Value]);

CREATE INDEX [IX_Lots_FabricPOId_FGPOId] ON [Lots] ([FabricPOId], [FGPOId]);

CREATE INDEX [IX_Lots_FGPOId] ON [Lots] ([FGPOId]);

CREATE UNIQUE INDEX [IX_Lots_LotNumber] ON [Lots] ([LotNumber]);

ALTER TABLE [FabricShipments] ADD CONSTRAINT [FK_FabricShipments_Lots_LotId] FOREIGN KEY ([LotId]) REFERENCES [Lots] ([ID]) ON DELETE SET NULL;

ALTER TABLE [MillProductions] ADD CONSTRAINT [FK_MillProductions_Lots_LotId] FOREIGN KEY ([LotId]) REFERENCES [Lots] ([ID]) ON DELETE SET NULL;

ALTER TABLE [MillTests] ADD CONSTRAINT [FK_MillTests_Lots_LotId] FOREIGN KEY ([LotId]) REFERENCES [Lots] ([ID]) ON DELETE SET NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260804200421_AddLotAndCatalogsAndAudit', N'9.0.10');

COMMIT;
GO

