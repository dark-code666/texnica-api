BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805154209_AddReceivingStatusCatalog'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ID', N'Active', N'Type', N'UpdatedAt', N'Value') AND [object_id] = OBJECT_ID(N'[CatalogValues]'))
        SET IDENTITY_INSERT [CatalogValues] ON;
    EXEC(N'INSERT INTO [CatalogValues] ([ID], [Active], [Type], [UpdatedAt], [Value])
    VALUES (42, CAST(1 AS bit), N''ReceivingStatus'', NULL, N''Pending''),
    (43, CAST(1 AS bit), N''ReceivingStatus'', NULL, N''Partially Received''),
    (44, CAST(1 AS bit), N''ReceivingStatus'', NULL, N''Fully Received''),
    (45, CAST(1 AS bit), N''ReceivingStatus'', NULL, N''Quantity Difference''),
    (46, CAST(1 AS bit), N''ReceivingStatus'', NULL, N''Rejected'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ID', N'Active', N'Type', N'UpdatedAt', N'Value') AND [object_id] = OBJECT_ID(N'[CatalogValues]'))
        SET IDENTITY_INSERT [CatalogValues] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805154209_AddReceivingStatusCatalog'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260805154209_AddReceivingStatusCatalog', N'9.0.10');
END;

COMMIT;
GO

