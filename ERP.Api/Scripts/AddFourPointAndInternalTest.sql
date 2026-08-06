BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805185054_AddFourPointAndInternalTest'
)
BEGIN
    CREATE TABLE [FourPointInspections] (
        [ID] int NOT NULL IDENTITY,
        [InspectionDate] datetime2 NOT NULL,
        [ReceivingId] int NULL,
        [ReceivingNumber] nvarchar(50) NULL,
        [FabricPOId] int NOT NULL,
        [FGPOId] int NOT NULL,
        [LotNumber] nvarchar(50) NULL,
        [LotId] int NULL,
        [RollNumber] nvarchar(50) NULL,
        [Width] decimal(18,4) NOT NULL,
        [InspectedLength] decimal(18,4) NOT NULL,
        [Points1] int NOT NULL,
        [Points2] int NOT NULL,
        [Points3] int NOT NULL,
        [Points4] int NOT NULL,
        [TotalPoints] AS CAST(([Points1] + (2 * [Points2]) + (3 * [Points3]) + (4 * [Points4])) AS int),
        [PointsPer100SqYd] AS CAST((CASE WHEN [Width] = 0 OR [InspectedLength] = 0 THEN 0 ELSE (([Points1] + (2 * [Points2]) + (3 * [Points3]) + (4 * [Points4])) * 3600.0) / ([Width] * [InspectedLength]) END) AS decimal(18,4)),
        [MaxAllowed] decimal(18,4) NOT NULL,
        [AcceptedQty] int NOT NULL,
        [RejectedQty] int NOT NULL,
        [HoldQty] int NOT NULL,
        [Result] nvarchar(50) NULL,
        [Inspector] nvarchar(100) NULL,
        [ReportLink] nvarchar(500) NULL,
        [Comments] nvarchar(1000) NULL,
        [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
        [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_FourPointInspections] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_FourPointInspections_FabricPOs_FabricPOId] FOREIGN KEY ([FabricPOId]) REFERENCES [FabricPOs] ([ID]) ON DELETE NO ACTION,
        CONSTRAINT [FK_FourPointInspections_FabricReceivings_ReceivingId] FOREIGN KEY ([ReceivingId]) REFERENCES [FabricReceivings] ([ID]) ON DELETE SET NULL,
        CONSTRAINT [FK_FourPointInspections_Fgpos_FGPOId] FOREIGN KEY ([FGPOId]) REFERENCES [Fgpos] ([ID]) ON DELETE NO ACTION,
        CONSTRAINT [FK_FourPointInspections_Lots_LotId] FOREIGN KEY ([LotId]) REFERENCES [Lots] ([ID]) ON DELETE SET NULL
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805185054_AddFourPointAndInternalTest'
)
BEGIN
    CREATE TABLE [InternalTests] (
        [ID] int NOT NULL IDENTITY,
        [TestDate] datetime2 NOT NULL,
        [FabricPOId] int NOT NULL,
        [FGPOId] int NOT NULL,
        [Supplier] nvarchar(100) NULL,
        [LotNumber] nvarchar(50) NULL,
        [LotId] int NULL,
        [Color] nvarchar(50) NULL,
        [ActualWidth] decimal(18,4) NOT NULL,
        [SpecimenAreaCm2] decimal(18,4) NOT NULL,
        [WeightBeforeG] decimal(18,4) NOT NULL,
        [WeightAfterG] decimal(18,4) NOT NULL,
        [TargetGSM] decimal(18,4) NOT NULL,
        [GsmBefore] AS CAST((CASE WHEN [SpecimenAreaCm2] = 0 THEN 0 ELSE ([WeightBeforeG] / ([SpecimenAreaCm2] / 10000.0)) END) AS decimal(18,4)),
        [GsmAfter] AS CAST((CASE WHEN [SpecimenAreaCm2] = 0 THEN 0 ELSE ([WeightAfterG] / ([SpecimenAreaCm2] / 10000.0)) END) AS decimal(18,4)),
        [GsmVariancePct] AS CAST((CASE WHEN [TargetGSM] = 0 OR [SpecimenAreaCm2] = 0 THEN 0 ELSE ((([WeightAfterG] / ([SpecimenAreaCm2] / 10000.0)) - [TargetGSM]) / [TargetGSM]) * 100 END) AS decimal(18,4)),
        [LengthBefore] decimal(18,4) NOT NULL,
        [LengthAfter] decimal(18,4) NOT NULL,
        [LengthShrinkagePct] AS CAST((CASE WHEN [LengthBefore] = 0 THEN 0 ELSE (([LengthBefore] - [LengthAfter]) / [LengthBefore]) * 100 END) AS decimal(18,4)),
        [WidthBefore] decimal(18,4) NOT NULL,
        [WidthAfter] decimal(18,4) NOT NULL,
        [WidthShrinkagePct] AS CAST((CASE WHEN [WidthBefore] = 0 THEN 0 ELSE (([WidthBefore] - [WidthAfter]) / [WidthBefore]) * 100 END) AS decimal(18,4)),
        [TorquePct] decimal(18,4) NOT NULL,
        [BowingPct] decimal(18,4) NOT NULL,
        [SkewingPct] decimal(18,4) NOT NULL,
        [ShadeResult] nvarchar(50) NULL,
        [WashAppearance] nvarchar(100) NULL,
        [HandFeel] nvarchar(100) NULL,
        [TestResult] nvarchar(50) NULL,
        [TestedBy] nvarchar(100) NULL,
        [ApprovedBy] nvarchar(100) NULL,
        [ReportLink] nvarchar(500) NULL,
        [Comments] nvarchar(1000) NULL,
        [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
        [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_InternalTests] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_InternalTests_FabricPOs_FabricPOId] FOREIGN KEY ([FabricPOId]) REFERENCES [FabricPOs] ([ID]) ON DELETE NO ACTION,
        CONSTRAINT [FK_InternalTests_Fgpos_FGPOId] FOREIGN KEY ([FGPOId]) REFERENCES [Fgpos] ([ID]) ON DELETE NO ACTION,
        CONSTRAINT [FK_InternalTests_Lots_LotId] FOREIGN KEY ([LotId]) REFERENCES [Lots] ([ID]) ON DELETE SET NULL
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805185054_AddFourPointAndInternalTest'
)
BEGIN
    CREATE INDEX [IX_FourPointInspections_FabricPOId] ON [FourPointInspections] ([FabricPOId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805185054_AddFourPointAndInternalTest'
)
BEGIN
    CREATE INDEX [IX_FourPointInspections_FGPOId_FabricPOId] ON [FourPointInspections] ([FGPOId], [FabricPOId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805185054_AddFourPointAndInternalTest'
)
BEGIN
    CREATE INDEX [IX_FourPointInspections_LotId] ON [FourPointInspections] ([LotId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805185054_AddFourPointAndInternalTest'
)
BEGIN
    CREATE INDEX [IX_FourPointInspections_ReceivingId] ON [FourPointInspections] ([ReceivingId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805185054_AddFourPointAndInternalTest'
)
BEGIN
    CREATE INDEX [IX_FourPointInspections_Result] ON [FourPointInspections] ([Result]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805185054_AddFourPointAndInternalTest'
)
BEGIN
    CREATE INDEX [IX_FourPointInspections_RollNumber] ON [FourPointInspections] ([RollNumber]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805185054_AddFourPointAndInternalTest'
)
BEGIN
    CREATE INDEX [IX_InternalTests_FabricPOId] ON [InternalTests] ([FabricPOId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805185054_AddFourPointAndInternalTest'
)
BEGIN
    CREATE INDEX [IX_InternalTests_FGPOId_FabricPOId] ON [InternalTests] ([FGPOId], [FabricPOId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805185054_AddFourPointAndInternalTest'
)
BEGIN
    CREATE INDEX [IX_InternalTests_LotId] ON [InternalTests] ([LotId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805185054_AddFourPointAndInternalTest'
)
BEGIN
    CREATE INDEX [IX_InternalTests_LotNumber] ON [InternalTests] ([LotNumber]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805185054_AddFourPointAndInternalTest'
)
BEGIN
    CREATE INDEX [IX_InternalTests_TestResult] ON [InternalTests] ([TestResult]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805185054_AddFourPointAndInternalTest'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260805185054_AddFourPointAndInternalTest', N'9.0.10');
END;

COMMIT;
GO

