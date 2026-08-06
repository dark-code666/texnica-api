BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805200018_AddShadeMatchAndInlineQuality'
)
BEGIN
    CREATE TABLE [InlineQualities] (
        [ID] int NOT NULL IDENTITY,
        [InspectionDate] datetime2 NOT NULL,
        [Time] nvarchar(20) NULL,
        [Line] nvarchar(50) NULL,
        [FGPOId] int NOT NULL,
        [Operation] nvarchar(100) NULL,
        [Operator] nvarchar(100) NULL,
        [CheckedQty] int NOT NULL,
        [CriticalDefects] int NOT NULL,
        [MajorDefects] int NOT NULL,
        [MinorDefects] int NOT NULL,
        [TotalDefects] AS CAST(([CriticalDefects] + [MajorDefects] + [MinorDefects]) AS int),
        [DhuPct] AS CAST((CASE WHEN [CheckedQty] = 0 THEN 0 ELSE (([CriticalDefects] + [MajorDefects] + [MinorDefects]) / CAST([CheckedQty] AS decimal(18,4))) * 100 END) AS decimal(18,4)),
        [DefectivePieces] int NOT NULL,
        [DefectiveRatePct] AS CAST((CASE WHEN [CheckedQty] = 0 THEN 0 ELSE ([DefectivePieces] / CAST([CheckedQty] AS decimal(18,4))) * 100 END) AS decimal(18,4)),
        [MaxAllowed] decimal(18,4) NOT NULL,
        [Result] nvarchar(50) NULL,
        [Inspector] nvarchar(100) NULL,
        [ImmediateCorrection] nvarchar(1000) NULL,
        [RootCause] nvarchar(1000) NULL,
        [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
        [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_InlineQualities] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_InlineQualities_Fgpos_FGPOId] FOREIGN KEY ([FGPOId]) REFERENCES [Fgpos] ([ID]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805200018_AddShadeMatchAndInlineQuality'
)
BEGIN
    CREATE TABLE [ShadeMatches] (
        [ID] int NOT NULL IDENTITY,
        [ReviewDate] datetime2 NOT NULL,
        [FGPOId] int NOT NULL,
        [BodyFabricLot] nvarchar(50) NULL,
        [RibLot] nvarchar(50) NULL,
        [ShoulderTapeLot] nvarchar(50) NULL,
        [BodyShadeGroup] nvarchar(50) NULL,
        [RibShadeGroup] nvarchar(50) NULL,
        [TapeShadeGroup] nvarchar(50) NULL,
        [BodyVsRib] nvarchar(100) NULL,
        [BodyVsTape] nvarchar(100) NULL,
        [LightSource] nvarchar(50) NULL,
        [BeforeWashResult] nvarchar(100) NULL,
        [AfterWashResult] nvarchar(100) NULL,
        [OverallResult] nvarchar(50) NULL,
        [ApprovedBy] nvarchar(100) NULL,
        [ReportLink] nvarchar(500) NULL,
        [Comments] nvarchar(1000) NULL,
        [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
        [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_ShadeMatches] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_ShadeMatches_Fgpos_FGPOId] FOREIGN KEY ([FGPOId]) REFERENCES [Fgpos] ([ID]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805200018_AddShadeMatchAndInlineQuality'
)
BEGIN
    CREATE INDEX [IX_InlineQualities_FGPOId] ON [InlineQualities] ([FGPOId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805200018_AddShadeMatchAndInlineQuality'
)
BEGIN
    CREATE INDEX [IX_InlineQualities_Line] ON [InlineQualities] ([Line]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805200018_AddShadeMatchAndInlineQuality'
)
BEGIN
    CREATE INDEX [IX_InlineQualities_Result] ON [InlineQualities] ([Result]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805200018_AddShadeMatchAndInlineQuality'
)
BEGIN
    CREATE INDEX [IX_ShadeMatches_BodyFabricLot] ON [ShadeMatches] ([BodyFabricLot]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805200018_AddShadeMatchAndInlineQuality'
)
BEGIN
    CREATE INDEX [IX_ShadeMatches_FGPOId] ON [ShadeMatches] ([FGPOId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805200018_AddShadeMatchAndInlineQuality'
)
BEGIN
    CREATE INDEX [IX_ShadeMatches_OverallResult] ON [ShadeMatches] ([OverallResult]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805200018_AddShadeMatchAndInlineQuality'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260805200018_AddShadeMatchAndInlineQuality', N'9.0.10');
END;

COMMIT;
GO

