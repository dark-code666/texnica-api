/* =====================================================================
   TPCS TEXNICA — SCRIPT DE DATOS DE PRUEBA (FLUJO COMPLETO)
   ---------------------------------------------------------------------
   Ejecutar en la BD local (DEAD666\TPCS) con la migración ya aplicada.
   Flujo: Customer → FGPO → Fabric Requirement → Fabric PO →
          Mill Production → Mill Test → Fabric Shipment
   Los IDs se capturan con SCOPE_IDENTITY() para mantener las FKs.
   ===================================================================== */

BEGIN TRY
    BEGIN TRANSACTION;

    /* ================================================================
       0) LIMPIEZA PREVIA (re-ejecutable) — borra solo los datos de prueba
       ================================================================ */
    DELETE FROM FabricShipments WHERE ShipmentNumber IN ('SHP-2026-001','SHP-2026-002');
    DELETE FROM MillTests        WHERE LotNumber   IN ('LOT-MILL-001','LOT-MILL-002');
    DELETE FROM MillProductions  WHERE LotNumber   IN ('LOT-MILL-001','LOT-MILL-002');
    DELETE FROM Lots             WHERE LotNumber   IN ('LOT-MILL-001','LOT-MILL-002');
    DELETE FROM FabricPOFgpos    WHERE FabricPOId IN (SELECT ID FROM FabricPOs WHERE FabricPONumber IN ('FABPO-2026-001','FABPO-2026-002'));
    DELETE FROM FabricPOs        WHERE FabricPONumber IN ('FABPO-2026-001','FABPO-2026-002');
    DELETE FROM FabricRequirements WHERE FGPOId IN (SELECT ID FROM Fgpos WHERE FGPONumber IN ('FPO-2026-001','FPO-2026-002'));
    DELETE FROM Fgpos            WHERE FGPONumber IN ('FPO-2026-001','FPO-2026-002');
    DELETE FROM Customers        WHERE Name = 'Royal Apparel Inc.';

    /* ================================================================
       1) CUSTOMER
       ================================================================ */
    DECLARE @CustomerId INT;
    INSERT INTO Customers (Name, Contact, Phone, Email, Address, Active, CreatedAt)
    VALUES ('Royal Apparel Inc.', 'John Smith', '+1-212-555-0100', 'john@royalapparel.com', '123 7th Ave, New York', 1, GETUTCDATE());
    SET @CustomerId = SCOPE_IDENTITY();

    /* ================================================================
       2) FGPO MASTER (2 órdenes)
       ================================================================ */
    DECLARE @Fgpo1 INT, @Fgpo2 INT;
    INSERT INTO Fgpos (FGPONumber, Status, CustomerId, Style, Color, OrderQuantity, DeliveryDate,
                       InTransitQty, ReceivedQty, TotalShippedQty, ShipmentVariance, PendingToShip,
                       OvershipmentQty, ProducedQty, ProductionVariance, PendingProduction,
                       OverproductionQty, DataOwner, Remarks, Active, CreatedAt)
    VALUES ('FPO-2026-001', 'In Progress', @CustomerId, 'ST-101', 'Navy', 50000, '2026-10-15',
            30000, 0, 0, 0, 0, 0, 30000, 0, 0, 0, 'TestUser', 'Orden de prueba', 1, GETUTCDATE());
    SET @Fgpo1 = SCOPE_IDENTITY();

    INSERT INTO Fgpos (FGPONumber, Status, CustomerId, Style, Color, OrderQuantity, DeliveryDate,
                       InTransitQty, ReceivedQty, TotalShippedQty, ShipmentVariance, PendingToShip,
                       OvershipmentQty, ProducedQty, ProductionVariance, PendingProduction,
                       OverproductionQty, DataOwner, Remarks, Active, CreatedAt)
    VALUES ('FPO-2026-002', 'Pending', @CustomerId, 'ST-102', 'Black', 35000, '2026-11-01',
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 'TestUser', 'Orden pendiente', 1, GETUTCDATE());
    SET @Fgpo2 = SCOPE_IDENTITY();

    /* ================================================================
       3) FABRIC REQUIREMENT (2 consumos del FGPO 1)
       ================================================================ */
    INSERT INTO FabricRequirements (FGPOId, Style, Color, FabricComponent, FabricDescription,
        Composition, GSM, RequiredWidth, UOM, OrderQuantity, ApprovedYield, GrossRequirement,
        AllowancePercentage, AllowanceQty, AvailableInventory, NetPurchaseRequirement,
        RequiredDate, Status, DataOwner, Remarks, Active, CreatedAt)
    VALUES (@Fgpo1, 'ST-101', 'Navy', 'Body Fabric', 'Jersey 100% Cotton', '100% Cotton',
            210, '58 in', 'Yards', 50000, 0.85, 58824, 2, 1176, 0, 60000,
            '2026-09-01', 'Pending', 'TestUser', 'Tela del cuerpo', 1, GETUTCDATE());

    INSERT INTO FabricRequirements (FGPOId, Style, Color, FabricComponent, FabricDescription,
        Composition, GSM, RequiredWidth, UOM, OrderQuantity, ApprovedYield, GrossRequirement,
        AllowancePercentage, AllowanceQty, AvailableInventory, NetPurchaseRequirement,
        RequiredDate, Status, DataOwner, Remarks, Active, CreatedAt)
    VALUES (@Fgpo1, 'ST-101', 'Navy', 'Rib', 'Rib 1x1', '95% Cotton 5% Spandex',
            280, '36 in', 'Yards', 5000, 0.80, 6250, 2, 125, 0, 6500,
            '2026-09-05', 'Pending', 'TestUser', 'Cuello y puños', 1, GETUTCDATE());

    /* ================================================================
       4) FABRIC PO (2 compras al molino)
       ================================================================ */
    DECLARE @Po1 INT, @Po2 INT;
    INSERT INTO FabricPOs (FabricPONumber, Supplier, FabricMill, FabricComponent, OrderedQuantity,
        UOM, UnitPrice, POAmount, OrderDate, RequiredCompletion, PlannedExport, PlannedArrival,
        POStatus, PurchaseOwner, ApprovedBy, LastUpdated, Remarks, Active, CreatedAt)
    VALUES ('FABPO-2026-001', 'TexMill Ltd.', 'TexMill Plant A', 'Body Fabric', 60000,
            'Yards', 3.25, 195000, '2026-07-15', '2026-08-30', '2026-08-25', '2026-09-10',
            'In Progress', 'TestUser', 'QA Manager', GETUTCDATE(), 'Compra principal', 1, GETUTCDATE());
    SET @Po1 = SCOPE_IDENTITY();

    INSERT INTO FabricPOs (FabricPONumber, Supplier, FabricMill, FabricComponent, OrderedQuantity,
        UOM, UnitPrice, POAmount, OrderDate, RequiredCompletion, PlannedExport, PlannedArrival,
        POStatus, PurchaseOwner, ApprovedBy, LastUpdated, Remarks, Active, CreatedAt)
    VALUES ('FABPO-2026-002', 'RibTex Co.', 'RibTex Plant B', 'Rib', 6500,
            'Yards', 2.80, 18200, '2026-07-20', '2026-09-05', '2026-09-01', '2026-09-15',
            'Pending', 'TestUser', 'QA Manager', GETUTCDATE(), 'Compra de rib', 1, GETUTCDATE());
    SET @Po2 = SCOPE_IDENTITY();

    /* ================================================================
       5) FABRIC PO - FGPO (asignación puente)
       ================================================================ */
    INSERT INTO FabricPOFgpos (FabricPOId, FGPOId, Style, Color, AllocatedQuantity, LastUpdated)
    VALUES (@Po1, @Fgpo1, 'ST-101', 'Navy', 50000, GETUTCDATE());

    INSERT INTO FabricPOFgpos (FabricPOId, FGPOId, Style, Color, AllocatedQuantity, LastUpdated)
    VALUES (@Po2, @Fgpo1, 'ST-101', 'Navy', 5000, GETUTCDATE());

    /* ================================================================
       6) LOTS (entidad de lote — nueva)
       ================================================================ */
    DECLARE @Lot1 INT, @Lot2 INT;
    INSERT INTO Lots (LotNumber, FabricPOId, FGPOId, ProducedQuantity, Active, CreatedAt)
    VALUES ('LOT-MILL-001', @Po1, @Fgpo1, 12000, 1, GETUTCDATE());
    SET @Lot1 = SCOPE_IDENTITY();

    INSERT INTO Lots (LotNumber, FabricPOId, FGPOId, ProducedQuantity, Active, CreatedAt)
    VALUES ('LOT-MILL-002', @Po1, @Fgpo1, 30000, 1, GETUTCDATE());
    SET @Lot2 = SCOPE_IDENTITY();

    /* ================================================================
       7) MILL PRODUCTION (2 lotes — Completion% es calculado por SQL)
       ================================================================ */
    INSERT INTO MillProductions (FabricPOId, FGPOId, Supplier, FabricComponent, Style, Color,
        PlannedQuantity, ProducedQuantity, LotNumber, LotId, RollQuantity, YardageOrQty, Weight,
        StartDate, FinishDate, PlannedExport, ActualExport, Status, DataOwner, Remarks, Active, CreatedAt)
    VALUES (@Po1, @Fgpo1, 'TexMill Ltd.', 'Body Fabric', 'ST-101', 'Navy',
        30000, 12000, 'LOT-MILL-001', @Lot1, 60, 12000, 2500,
        '2026-08-01', NULL, '2026-08-20', NULL, 'In Progress', 'TestUser', 'Lote en producción', 1, GETUTCDATE());

    INSERT INTO MillProductions (FabricPOId, FGPOId, Supplier, FabricComponent, Style, Color,
        PlannedQuantity, ProducedQuantity, LotNumber, LotId, RollQuantity, YardageOrQty, Weight,
        StartDate, FinishDate, PlannedExport, ActualExport, Status, DataOwner, Remarks, Active, CreatedAt)
    VALUES (@Po1, @Fgpo1, 'TexMill Ltd.', 'Body Fabric', 'ST-101', 'Navy',
        30000, 30000, 'LOT-MILL-002', @Lot2, 55, 30000, 6200,
        '2026-08-01', '2026-08-15', '2026-08-20', '2026-08-18', 'Completed', 'TestUser', 'Lote completado', 1, GETUTCDATE());

    /* ================================================================
       8) MILL TEST (1 por lote)
       ================================================================ */
    INSERT INTO MillTests (FabricPOId, FGPOId, Supplier, LotNumber, LotId, Color,
        RollQty, ActualWidth, ActualGSM, LengthShrinkagePercentage, WidthShrinkagePercentage,
        TorquePercentage, BowingPercentage, SkewingPercentage, Colorfastness, WashAppearance,
        HandFeel, TestDate, TestedBy, TestResult, ApprovedForExport, ReportLink, Comments,
        Active, CreatedAt)
    VALUES (@Po1, @Fgpo1, 'TexMill Ltd.', 'LOT-MILL-001', @Lot1, 'Navy',
        5, 58, 208, 2.5, 1.8, 3.0, 1.2, 0.8, '4.5', 'Good', 'Soft',
        '2026-08-03', 'QA Lab', 'Passed', 1, 'https://reports/lot-001.pdf', 'Todo dentro de tolerancia',
        1, GETUTCDATE());

    INSERT INTO MillTests (FabricPOId, FGPOId, Supplier, LotNumber, LotId, Color,
        RollQty, ActualWidth, ActualGSM, LengthShrinkagePercentage, WidthShrinkagePercentage,
        TorquePercentage, BowingPercentage, SkewingPercentage, Colorfastness, WashAppearance,
        HandFeel, TestDate, TestedBy, TestResult, ApprovedForExport, ReportLink, Comments,
        Active, CreatedAt)
    VALUES (@Po1, @Fgpo1, 'TexMill Ltd.', 'LOT-MILL-002', @Lot2, 'Navy',
        4, 57, 211, 4.8, 3.2, 6.5, 2.1, 1.5, '3.5', 'Fair', 'Stiff',
        '2026-08-16', 'QA Lab', 'Conditionally Passed', 0, NULL, 'Requiere re-test de torque',
        1, GETUTCDATE());

    /* ================================================================
       9) FABRIC SHIPMENT (1 en tránsito + 1 entregado — cantidades calculadas)
       ================================================================ */
    INSERT INTO FabricShipments (ShipmentNumber, FabricPOId, FGPOId, Supplier, LotNumber, LotId,
        RollQty, ShippedQuantity, UOM, ShippedWeight, PackingList, InvoiceNumber, ContainerAWB,
        ShippingMethod, ETD, ETA, ShipmentStatus, DeliveredToTexnicaDate, DataOwner, Remarks,
        Active, CreatedAt)
    VALUES ('SHP-2026-001', @Po1, @Fgpo1, 'TexMill Ltd.', 'LOT-MILL-001', @Lot1,
        50, 30000, 'Yards', 6200, 'PL-001', 'INV-9001', 'MSKU-1234567',
        'Sea', '2026-08-10', '2026-08-30', 'In Transit', NULL, 'TestUser', 'Primer embarque',
        1, GETUTCDATE());

    INSERT INTO FabricShipments (ShipmentNumber, FabricPOId, FGPOId, Supplier, LotNumber, LotId,
        RollQty, ShippedQuantity, UOM, ShippedWeight, PackingList, InvoiceNumber, ContainerAWB,
        ShippingMethod, ETD, ETA, ShipmentStatus, DeliveredToTexnicaDate, DataOwner, Remarks,
        Active, CreatedAt)
    VALUES ('SHP-2026-002', @Po1, @Fgpo1, 'TexMill Ltd.', 'LOT-MILL-002', @Lot2,
        40, 30000, 'Yards', 6200, 'PL-002', 'INV-9002', 'MSKU-7654321',
        'Sea', '2026-08-10', '2026-08-25', 'Delivered', '2026-08-25', 'TestUser', 'Segundo embarque',
        1, GETUTCDATE());

    /* ================================================================
       VERIFICACIÓN — lo que deberías ver
       ================================================================ */
    PRINT '==============================================';
    PRINT 'DATOS DE PRUEBA INSERTADOS CORRECTAMENTE ✅';
    PRINT '==============================================';
    SELECT 'Customer' AS Tabla, COUNT(*) AS Filas FROM Customers WHERE Name='Royal Apparel Inc.'
    UNION ALL SELECT 'Fgpos', COUNT(*) FROM Fgpos WHERE FGPONumber IN ('FPO-2026-001','FPO-2026-002')
    UNION ALL SELECT 'FabricRequirements', COUNT(*) FROM FabricRequirements WHERE FGPOId=@Fgpo1
    UNION ALL SELECT 'FabricPOs', COUNT(*) FROM FabricPOs WHERE FabricPONumber IN ('FABPO-2026-001','FABPO-2026-002')
    UNION ALL SELECT 'FabricPOFgpos', COUNT(*) FROM FabricPOFgpos WHERE FabricPOId IN (@Po1,@Po2)
    UNION ALL SELECT 'Lots', COUNT(*) FROM Lots WHERE LotNumber IN ('LOT-MILL-001','LOT-MILL-002')
    UNION ALL SELECT 'MillProductions', COUNT(*) FROM MillProductions WHERE LotNumber IN ('LOT-MILL-001','LOT-MILL-002')
    UNION ALL SELECT 'MillTests', COUNT(*) FROM MillTests WHERE LotNumber IN ('LOT-MILL-001','LOT-MILL-002')
    UNION ALL SELECT 'FabricShipments', COUNT(*) FROM FabricShipments WHERE ShipmentNumber IN ('SHP-2026-001','SHP-2026-002');

    PRINT '';
    PRINT 'Comprobación de columnas calculadas (SQL):';
    SELECT LotNumber, PlannedQuantity, ProducedQuantity, CompletionPercentage
    FROM MillProductions WHERE LotNumber IN ('LOT-MILL-001','LOT-MILL-002');
    SELECT ShipmentNumber, ShipmentStatus, ShippedQuantity, InTransitQuantity, RemainingToDeliver
    FROM FabricShipments WHERE ShipmentNumber IN ('SHP-2026-001','SHP-2026-002');

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT 'ERROR: ' + ERROR_MESSAGE();
    THROW;
END CATCH;
GO
