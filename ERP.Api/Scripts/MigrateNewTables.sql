BEGIN TRANSACTION;
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

DECLARE @var sysname;
SELECT @var = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MillTests]') AND [c].[name] = N'CreatedAt');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [MillTests] DROP CONSTRAINT [' + @var + '];');
ALTER TABLE [MillTests] ADD DEFAULT (GETUTCDATE()) FOR [CreatedAt];

ALTER TABLE [MillTests] ADD [LotId] int NULL;

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MillProductions]') AND [c].[name] = N'CreatedAt');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [MillProductions] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [MillProductions] ADD DEFAULT (GETUTCDATE()) FOR [CreatedAt];

ALTER TABLE [MillProductions] ADD [LotId] int NULL;

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Fgpos]') AND [c].[name] = N'CreatedAt');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Fgpos] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Fgpos] ADD DEFAULT (GETUTCDATE()) FOR [CreatedAt];

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Factories]') AND [c].[name] = N'CreatedAt');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Factories] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Factories] ADD DEFAULT (GETUTCDATE()) FOR [CreatedAt];

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[FabricShipments]') AND [c].[name] = N'CreatedAt');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [FabricShipments] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [FabricShipments] ADD DEFAULT (GETUTCDATE()) FOR [CreatedAt];

ALTER TABLE [FabricShipments] ADD [LotId] int NULL;

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[FabricRequirements]') AND [c].[name] = N'CreatedAt');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [FabricRequirements] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [FabricRequirements] ADD DEFAULT (GETUTCDATE()) FOR [CreatedAt];

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[FabricPOs]') AND [c].[name] = N'CreatedAt');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [FabricPOs] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [FabricPOs] ADD DEFAULT (GETUTCDATE()) FOR [CreatedAt];

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Customers]') AND [c].[name] = N'CreatedAt');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Customers] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [Customers] ADD DEFAULT (GETUTCDATE()) FOR [CreatedAt];

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MillProductions]') AND [c].[name] = N'CompletionPercentage');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [MillProductions] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [MillProductions] DROP COLUMN [CompletionPercentage];
ALTER TABLE [MillProductions] ADD [CompletionPercentage] AS CAST((CASE WHEN [PlannedQuantity] = 0 THEN 0 ELSE ([ProducedQuantity] / [PlannedQuantity]) * 100 END) AS decimal(18,4));

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[FabricShipments]') AND [c].[name] = N'RemainingToDeliver');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [FabricShipments] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [FabricShipments] DROP COLUMN [RemainingToDeliver];
ALTER TABLE [FabricShipments] ADD [RemainingToDeliver] AS CAST((CASE WHEN [DeliveredToTexnicaDate] IS NULL THEN [ShippedQuantity] ELSE 0 END) AS decimal(18,4));

DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[FabricShipments]') AND [c].[name] = N'InTransitQuantity');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [FabricShipments] DROP CONSTRAINT [' + @var10 + '];');
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

