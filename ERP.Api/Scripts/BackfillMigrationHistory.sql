/* =====================================================================
   RESPALDO DEL HISTORIAL DE MIGRACIONES (BD EN LA NUBE)
   ---------------------------------------------------------------------
   La BD en la nube tiene las tablas ANTIGUAS pero su __EFMigrationsHistory
   está vacío. Este script marca como aplicadas las 12 migraciones viejas
   (cuyas tablas ya existen), para que el historial quede consistente.

   CORRER ANTES de MigrateNewTables.sql
   (MigrateNewTables agrega las 3 migraciones nuevas y su historial).

   Es seguro re-ejecutarlo (solo inserta las que falten).
   ===================================================================== */

IF OBJECT_ID('dbo.__EFMigrationsHistory', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[__EFMigrationsHistory] (
        [MigrationId]    nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32)  NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
SELECT [MigrationId], [ProductVersion]
FROM (
    VALUES
        (N'20260730204321_InitialCreate',                          N'9.0.10'),
        (N'20260730221504_TableUser',                              N'9.0.10'),
        (N'20260731144917_UpdateUserTable',                        N'9.0.10'),
        (N'20260731221143_AddRolesAndPermissions',                 N'9.0.10'),
        (N'20260802002213_AddMustChangePasswordToUser',            N'9.0.10'),
        (N'20260803155118_AddFgpoTable',                           N'9.0.10'),
        (N'20260803173344_AddCustomersAndFactories',               N'9.0.10'),
        (N'20260803195928_AddFabricRequirementTable',              N'9.0.10'),
        (N'20260803204448_FgpoMasterSpecUpdate',                   N'9.0.10'),
        (N'20260803221341_AddFabricPO',                            N'9.0.10'),
        (N'20260803225707_AddFabricPOFgpoAllocatedQuantity',       N'9.0.10'),
        (N'20260804134823_FabricPOFgpoEntityRefactor',             N'9.0.10')
) AS M ([MigrationId], [ProductVersion])
WHERE NOT EXISTS (
    SELECT 1 FROM [dbo].[__EFMigrationsHistory] H
    WHERE H.[MigrationId] = M.[MigrationId]
);
GO
