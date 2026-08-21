SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF NOT EXISTS (
    SELECT 1
    FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260821161115_AddUserTypeAndCustomerAssignment'
)
BEGIN
    IF COL_LENGTH(N'[Users]', N'CustomerId') IS NULL
    BEGIN
        ALTER TABLE [Users] ADD [CustomerId] int NULL;
    END;

    IF COL_LENGTH(N'[Users]', N'UserType') IS NULL
    BEGIN
        ALTER TABLE [Users]
            ADD [UserType] nvarchar(20) NOT NULL
                CONSTRAINT [DF_Users_UserType] DEFAULT N'Employee';
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM sys.indexes
        WHERE [name] = N'IX_Users_CustomerId'
          AND [object_id] = OBJECT_ID(N'[Users]')
    )
    BEGIN
        CREATE INDEX [IX_Users_CustomerId]
            ON [Users] ([CustomerId]);
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM sys.foreign_keys
        WHERE [name] = N'FK_Users_Customers_CustomerId'
          AND [parent_object_id] = OBJECT_ID(N'[Users]')
    )
    BEGIN
        ALTER TABLE [Users]
            ADD CONSTRAINT [FK_Users_Customers_CustomerId]
            FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([ID])
            ON DELETE NO ACTION;
    END;

    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260821161115_AddUserTypeAndCustomerAssignment', N'9.0.10');
END;

COMMIT;
GO
