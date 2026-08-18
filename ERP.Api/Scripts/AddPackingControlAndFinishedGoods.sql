BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260812193534_AddPackingControlAndFinishedGoods'
)
BEGIN
    CREATE TABLE [FinishedGoods] (
        [ID] int NOT NULL IDENTITY,
        [ReceiptDate] datetime2 NOT NULL,
        [FGPOId] int NOT NULL,
        [PackedQty] decimal(18,4) NOT NULL,
        [WarehouseReceived] decimal(18,4) NOT NULL,
        [ReservedForShipment] decimal(18,4) NOT NULL,
        [LoadedQty] decimal(18,4) NOT NULL,
        [ShippedQty] decimal(18,4) NOT NULL,
        [ReadyToShipQty] AS CAST((CASE WHEN [WarehouseReceived] - [ReservedForShipment] - [LoadedQty] - [ShippedQty] > 0 THEN [WarehouseReceived] - [ReservedForShipment] - [LoadedQty] - [ShippedQty] ELSE 0 END) AS decimal(18,4)),
        [WarehouseBalance] AS CAST((CASE WHEN [WarehouseReceived] - [LoadedQty] - [ShippedQty] > 0 THEN [WarehouseReceived] - [LoadedQty] - [ShippedQty] ELSE 0 END) AS decimal(18,4)),
        [WarehouseLocation] nvarchar(150) NULL,
        [Status] nvarchar(100) NULL,
        [DataOwnerId] int NULL,
        [LastUpdated] datetime2 NULL,
        [Remarks] nvarchar(1000) NULL,
        [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
        [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_FinishedGoods] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_FinishedGoods_Fgpos_FGPOId] FOREIGN KEY ([FGPOId]) REFERENCES [Fgpos] ([ID]) ON DELETE NO ACTION,
        CONSTRAINT [FK_FinishedGoods_Users_DataOwnerId] FOREIGN KEY ([DataOwnerId]) REFERENCES [Users] ([ID]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260812193534_AddPackingControlAndFinishedGoods'
)
BEGIN
    CREATE TABLE [PackingControls] (
        [ID] int NOT NULL IDENTITY,
        [PackingDate] datetime2 NOT NULL,
        [FGPOId] int NOT NULL,
        [QcPassedQty] decimal(18,4) NOT NULL,
        [ReceivedByPackingQty] decimal(18,4) NOT NULL,
        [FoldedQty] decimal(18,4) NOT NULL,
        [PolybaggedQty] decimal(18,4) NOT NULL,
        [PackedQty] decimal(18,4) NOT NULL,
        [FullCartons] int NOT NULL,
        [PartialCartons] int NOT NULL,
        [PcsPerCarton] int NOT NULL,
        [ReadyToShipQty] AS CAST([PackedQty] AS decimal(18,4)),
        [PackingVariance] AS CAST(([PackedQty] - [QcPassedQty]) AS decimal(18,4)),
        [PendingPacking] AS CAST((CASE WHEN [QcPassedQty] - [PackedQty] > 0 THEN [QcPassedQty] - [PackedQty] ELSE 0 END) AS decimal(18,4)),
        [OverpackedQty] AS CAST((CASE WHEN [PackedQty] - [QcPassedQty] > 0 THEN [PackedQty] - [QcPassedQty] ELSE 0 END) AS decimal(18,4)),
        [ResponsiblePersonId] int NULL,
        [LastUpdated] datetime2 NULL,
        [Remarks] nvarchar(1000) NULL,
        [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
        [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_PackingControls] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_PackingControls_Fgpos_FGPOId] FOREIGN KEY ([FGPOId]) REFERENCES [Fgpos] ([ID]) ON DELETE NO ACTION,
        CONSTRAINT [FK_PackingControls_Users_ResponsiblePersonId] FOREIGN KEY ([ResponsiblePersonId]) REFERENCES [Users] ([ID]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260812193534_AddPackingControlAndFinishedGoods'
)
BEGIN
    CREATE INDEX [IX_FinishedGoods_DataOwnerId] ON [FinishedGoods] ([DataOwnerId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260812193534_AddPackingControlAndFinishedGoods'
)
BEGIN
    CREATE INDEX [IX_FinishedGoods_FGPOId] ON [FinishedGoods] ([FGPOId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260812193534_AddPackingControlAndFinishedGoods'
)
BEGIN
    CREATE INDEX [IX_FinishedGoods_ReceiptDate] ON [FinishedGoods] ([ReceiptDate]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260812193534_AddPackingControlAndFinishedGoods'
)
BEGIN
    CREATE INDEX [IX_FinishedGoods_Status] ON [FinishedGoods] ([Status]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260812193534_AddPackingControlAndFinishedGoods'
)
BEGIN
    CREATE INDEX [IX_PackingControls_FGPOId] ON [PackingControls] ([FGPOId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260812193534_AddPackingControlAndFinishedGoods'
)
BEGIN
    CREATE INDEX [IX_PackingControls_PackingDate] ON [PackingControls] ([PackingDate]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260812193534_AddPackingControlAndFinishedGoods'
)
BEGIN
    CREATE INDEX [IX_PackingControls_ResponsiblePersonId] ON [PackingControls] ([ResponsiblePersonId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260812193534_AddPackingControlAndFinishedGoods'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260812193534_AddPackingControlAndFinishedGoods', N'9.0.10');
END;

COMMIT;
GO

