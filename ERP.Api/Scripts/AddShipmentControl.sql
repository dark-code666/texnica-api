BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260812203303_AddShipmentControl'
)
BEGIN
    CREATE TABLE [ShipmentControls] (
        [ID] int NOT NULL IDENTITY,
        [ShipmentNumber] nvarchar(100) NOT NULL,
        [PlannedLoadingDate] datetime2 NULL,
        [ActualLoadingDate] datetime2 NULL,
        [ETD] datetime2 NULL,
        [ETA] datetime2 NULL,
        [FGPOId] int NOT NULL,
        [PlannedQty] decimal(18,4) NOT NULL,
        [ActualLoadedQty] decimal(18,4) NOT NULL,
        [InTransitQty] decimal(18,4) NOT NULL,
        [CustomerReceivedQty] decimal(18,4) NOT NULL,
        [TotalShippedQty] decimal(18,4) NOT NULL,
        [ShipmentVariance] AS CAST(([TotalShippedQty] - [PlannedQty]) AS decimal(18,4)),
        [PendingToShip] AS CAST((CASE WHEN [PlannedQty] - [TotalShippedQty] > 0 THEN [PlannedQty] - [TotalShippedQty] ELSE 0 END) AS decimal(18,4)),
        [OvershipmentQty] AS CAST((CASE WHEN [TotalShippedQty] - [PlannedQty] > 0 THEN [TotalShippedQty] - [PlannedQty] ELSE 0 END) AS decimal(18,4)),
        [ContainerType] nvarchar(100) NULL,
        [ContainerNumber] nvarchar(100) NULL,
        [BookingNumber] nvarchar(100) NULL,
        [Destination] nvarchar(200) NULL,
        [ShipmentStatus] nvarchar(100) NULL,
        [PackingList] nvarchar(300) NULL,
        [InvoiceNumber] nvarchar(100) NULL,
        [LoadPlan] nvarchar(500) NULL,
        [DataOwnerId] int NULL,
        [LastUpdated] datetime2 NULL,
        [Remarks] nvarchar(1000) NULL,
        [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
        [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_ShipmentControls] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_ShipmentControls_Fgpos_FGPOId] FOREIGN KEY ([FGPOId]) REFERENCES [Fgpos] ([ID]) ON DELETE NO ACTION,
        CONSTRAINT [FK_ShipmentControls_Users_DataOwnerId] FOREIGN KEY ([DataOwnerId]) REFERENCES [Users] ([ID]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260812203303_AddShipmentControl'
)
BEGIN
    CREATE INDEX [IX_ShipmentControls_DataOwnerId] ON [ShipmentControls] ([DataOwnerId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260812203303_AddShipmentControl'
)
BEGIN
    CREATE INDEX [IX_ShipmentControls_FGPOId] ON [ShipmentControls] ([FGPOId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260812203303_AddShipmentControl'
)
BEGIN
    CREATE INDEX [IX_ShipmentControls_ShipmentNumber] ON [ShipmentControls] ([ShipmentNumber]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260812203303_AddShipmentControl'
)
BEGIN
    CREATE INDEX [IX_ShipmentControls_ShipmentStatus] ON [ShipmentControls] ([ShipmentStatus]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260812203303_AddShipmentControl'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260812203303_AddShipmentControl', N'9.0.10');
END;

COMMIT;
GO

