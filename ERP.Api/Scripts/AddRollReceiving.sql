BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805154414_AddRollReceiving'
)
BEGIN
    CREATE TABLE [RollReceivings] (
        [ID] int NOT NULL IDENTITY,
        [ReceivingId] int NOT NULL,
        [ReceivingNumber] nvarchar(50) NULL,
        [FabricPOId] int NOT NULL,
        [FGPOId] int NOT NULL,
        [Supplier] nvarchar(100) NULL,
        [LotNumber] nvarchar(50) NULL,
        [LotId] int NULL,
        [RollNumber] nvarchar(50) NULL,
        [SupplierRollNumber] nvarchar(50) NULL,
        [Color] nvarchar(50) NULL,
        [GrossWeight] decimal(18,4) NOT NULL,
        [NetWeight] decimal(18,4) NOT NULL,
        [ActualYardage] decimal(18,4) NOT NULL,
        [ActualWidth] decimal(18,4) NOT NULL,
        [ActualGSM] decimal(18,4) NOT NULL,
        [ShadeGroup] nvarchar(50) NULL,
        [DamagedQty] decimal(18,4) NOT NULL,
        [Condition] nvarchar(100) NULL,
        [WarehouseLocation] nvarchar(100) NULL,
        [ReceivedDate] datetime2 NOT NULL,
        [DataOwner] nvarchar(100) NULL,
        [Comments] nvarchar(1000) NULL,
        [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
        [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_RollReceivings] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_RollReceivings_FabricPOs_FabricPOId] FOREIGN KEY ([FabricPOId]) REFERENCES [FabricPOs] ([ID]) ON DELETE NO ACTION,
        CONSTRAINT [FK_RollReceivings_FabricReceivings_ReceivingId] FOREIGN KEY ([ReceivingId]) REFERENCES [FabricReceivings] ([ID]) ON DELETE NO ACTION,
        CONSTRAINT [FK_RollReceivings_Fgpos_FGPOId] FOREIGN KEY ([FGPOId]) REFERENCES [Fgpos] ([ID]) ON DELETE NO ACTION,
        CONSTRAINT [FK_RollReceivings_Lots_LotId] FOREIGN KEY ([LotId]) REFERENCES [Lots] ([ID]) ON DELETE SET NULL
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805154414_AddRollReceiving'
)
BEGIN
    CREATE INDEX [IX_RollReceivings_FabricPOId] ON [RollReceivings] ([FabricPOId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805154414_AddRollReceiving'
)
BEGIN
    CREATE INDEX [IX_RollReceivings_FGPOId_FabricPOId] ON [RollReceivings] ([FGPOId], [FabricPOId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805154414_AddRollReceiving'
)
BEGIN
    CREATE INDEX [IX_RollReceivings_LotId] ON [RollReceivings] ([LotId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805154414_AddRollReceiving'
)
BEGIN
    CREATE INDEX [IX_RollReceivings_LotNumber] ON [RollReceivings] ([LotNumber]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805154414_AddRollReceiving'
)
BEGIN
    CREATE INDEX [IX_RollReceivings_ReceivingId] ON [RollReceivings] ([ReceivingId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805154414_AddRollReceiving'
)
BEGIN
    CREATE INDEX [IX_RollReceivings_RollNumber] ON [RollReceivings] ([RollNumber]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805154414_AddRollReceiving'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260805154414_AddRollReceiving', N'9.0.10');
END;

COMMIT;
GO

