BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811210046_AddTrimsSupplierRelation'
)
BEGIN
    DECLARE @var sysname;
    SELECT @var = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TrimsControls]') AND [c].[name] = N'Supplier');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [TrimsControls] DROP CONSTRAINT [' + @var + '];');
    ALTER TABLE [TrimsControls] DROP COLUMN [Supplier];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811210046_AddTrimsSupplierRelation'
)
BEGIN
    ALTER TABLE [TrimsControls] ADD [SupplierId] int NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811210046_AddTrimsSupplierRelation'
)
BEGIN
    CREATE INDEX [IX_TrimsControls_SupplierId] ON [TrimsControls] ([SupplierId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811210046_AddTrimsSupplierRelation'
)
BEGIN
    ALTER TABLE [TrimsControls] ADD CONSTRAINT [FK_TrimsControls_Suppliers_SupplierId] FOREIGN KEY ([SupplierId]) REFERENCES [Suppliers] ([ID]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811210046_AddTrimsSupplierRelation'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260811210046_AddTrimsSupplierRelation', N'9.0.10');
END;

COMMIT;
GO

