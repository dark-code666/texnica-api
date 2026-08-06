BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805153958_AddFabricReceiving'
)
BEGIN
    CREATE TABLE [FabricReceivings] (
        [ID] int NOT NULL IDENTITY,
        [ReceivingNumber] nvarchar(50) NOT NULL,
        [ReceivingDate] datetime2 NOT NULL,
        [ShipmentNumber] nvarchar(50) NULL,
        [FabricPOId] int NOT NULL,
        [FGPOId] int NOT NULL,
        [Supplier] nvarchar(100) NULL,
        [PackingListQty] decimal(18,4) NOT NULL,
        [ActualReceivedQty] decimal(18,4) NOT NULL,
        [ReceivingVariance] AS CAST(([ActualReceivedQty] - [PackingListQty]) AS decimal(18,4)),
        [ReceivingShortage] AS CAST((CASE WHEN [PackingListQty] > [ActualReceivedQty] THEN [PackingListQty] - [ActualReceivedQty] ELSE 0 END) AS decimal(18,4)),
        [ReceivingOverQty] AS CAST((CASE WHEN [ActualReceivedQty] > [PackingListQty] THEN [ActualReceivedQty] - [PackingListQty] ELSE 0 END) AS decimal(18,4)),
        [ExpectedRolls] int NOT NULL,
        [ReceivedRolls] int NOT NULL,
        [MissingRolls] AS CAST((CASE WHEN [ExpectedRolls] > [ReceivedRolls] THEN [ExpectedRolls] - [ReceivedRolls] ELSE 0 END) AS int),
        [ReceivingStatus] nvarchar(50) NULL,
        [WarehouseLocation] nvarchar(100) NULL,
        [ReceivedBy] nvarchar(100) NULL,
        [DataOwner] nvarchar(100) NULL,
        [Remarks] nvarchar(1000) NULL,
        [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
        [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_FabricReceivings] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_FabricReceivings_FabricPOs_FabricPOId] FOREIGN KEY ([FabricPOId]) REFERENCES [FabricPOs] ([ID]) ON DELETE NO ACTION,
        CONSTRAINT [FK_FabricReceivings_Fgpos_FGPOId] FOREIGN KEY ([FGPOId]) REFERENCES [Fgpos] ([ID]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805153958_AddFabricReceiving'
)
BEGIN
    CREATE INDEX [IX_FabricReceivings_FabricPOId] ON [FabricReceivings] ([FabricPOId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805153958_AddFabricReceiving'
)
BEGIN
    CREATE INDEX [IX_FabricReceivings_FGPOId_FabricPOId] ON [FabricReceivings] ([FGPOId], [FabricPOId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805153958_AddFabricReceiving'
)
BEGIN
    CREATE UNIQUE INDEX [IX_FabricReceivings_ReceivingNumber] ON [FabricReceivings] ([ReceivingNumber]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805153958_AddFabricReceiving'
)
BEGIN
    CREATE INDEX [IX_FabricReceivings_ReceivingStatus] ON [FabricReceivings] ([ReceivingStatus]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805153958_AddFabricReceiving'
)
BEGIN
    CREATE INDEX [IX_FabricReceivings_ShipmentNumber] ON [FabricReceivings] ([ShipmentNumber]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805153958_AddFabricReceiving'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260805153958_AddFabricReceiving', N'9.0.10');
END;

COMMIT;
GO

