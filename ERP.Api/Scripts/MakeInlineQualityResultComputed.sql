BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805212111_MakeInlineQualityResultComputed'
)
BEGIN
    DROP INDEX [IX_InlineQualities_Result] ON [InlineQualities];
    DECLARE @var sysname;
    SELECT @var = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[InlineQualities]') AND [c].[name] = N'Result');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [InlineQualities] DROP CONSTRAINT [' + @var + '];');
    ALTER TABLE [InlineQualities] DROP COLUMN [Result];
    EXEC(N'ALTER TABLE [InlineQualities] ADD [Result] AS CAST((CASE WHEN [CheckedQty] = 0 THEN ''Pending'' WHEN ([CriticalDefects] + [MajorDefects] + [MinorDefects]) / CAST([CheckedQty] AS decimal(18,4)) * 100 > [MaxAllowed] THEN ''Failed'' ELSE ''Passed'' END) AS nvarchar(50))');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805212111_MakeInlineQualityResultComputed'
)
BEGIN
    CREATE INDEX [IX_InlineQualities_Result] ON [InlineQualities] ([Result]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805212111_MakeInlineQualityResultComputed'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260805212111_MakeInlineQualityResultComputed', N'9.0.10');
END;

COMMIT;
GO

