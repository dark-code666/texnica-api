using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ERP.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddLotAndCatalogsAndAudit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MillTests_FGPOId",
                table: "MillTests");

            migrationBuilder.DropIndex(
                name: "IX_MillProductions_FGPOId",
                table: "MillProductions");

            migrationBuilder.DropIndex(
                name: "IX_FabricShipments_FGPOId",
                table: "FabricShipments");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Roles",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Roles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "RolePermissions",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "RolePermissions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Permissions",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Permissions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "MillTests",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "LotId",
                table: "MillTests",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "MillProductions",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "LotId",
                table: "MillProductions",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Fgpos",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Factories",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "FabricShipments",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "LotId",
                table: "FabricShipments",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "FabricRequirements",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "FabricPOs",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Customers",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<decimal>(
                name: "CompletionPercentage",
                table: "MillProductions",
                type: "decimal(18,2)",
                nullable: false,
                computedColumnSql: "CAST((CASE WHEN [PlannedQuantity] = 0 THEN 0 ELSE ([ProducedQuantity] / [PlannedQuantity]) * 100 END) AS decimal(18,4))",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldPrecision: 18,
                oldScale: 4);

            migrationBuilder.AlterColumn<decimal>(
                name: "RemainingToDeliver",
                table: "FabricShipments",
                type: "decimal(18,2)",
                nullable: false,
                computedColumnSql: "CAST((CASE WHEN [DeliveredToTexnicaDate] IS NULL THEN [ShippedQuantity] ELSE 0 END) AS decimal(18,4))",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldPrecision: 18,
                oldScale: 4);

            migrationBuilder.AlterColumn<decimal>(
                name: "InTransitQuantity",
                table: "FabricShipments",
                type: "decimal(18,2)",
                nullable: false,
                computedColumnSql: "CAST((CASE WHEN [DeliveredToTexnicaDate] IS NULL THEN [ShippedQuantity] ELSE 0 END) AS decimal(18,4))",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldPrecision: 18,
                oldScale: 4);

            migrationBuilder.CreateTable(
                name: "CatalogValues",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogValues", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Lots",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LotNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FabricPOId = table.Column<int>(type: "int", nullable: false),
                    FGPOId = table.Column<int>(type: "int", nullable: false),
                    ProducedQuantity = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lots", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Lots_FabricPOs_FabricPOId",
                        column: x => x.FabricPOId,
                        principalTable: "FabricPOs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lots_Fgpos_FGPOId",
                        column: x => x.FGPOId,
                        principalTable: "Fgpos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "CatalogValues",
                columns: new[] { "ID", "Active", "Type", "UpdatedAt", "Value" },
                values: new object[,]
                {
                    { 1, true, "UOM", null, "Yards" },
                    { 2, true, "UOM", null, "Meters" },
                    { 3, true, "UOM", null, "Kilograms" },
                    { 4, true, "UOM", null, "Pounds" },
                    { 5, true, "UOM", null, "Rolls" },
                    { 6, true, "UOM", null, "Pieces" },
                    { 7, true, "FabricComponent", null, "Body Fabric" },
                    { 8, true, "FabricComponent", null, "Rib" },
                    { 9, true, "FabricComponent", null, "Shoulder Tape" },
                    { 10, true, "FabricComponent", null, "Neck Tape" },
                    { 11, true, "FabricComponent", null, "Pocketing" },
                    { 12, true, "FabricComponent", null, "Other" },
                    { 13, true, "ProductionStatus", null, "Not Started" },
                    { 14, true, "ProductionStatus", null, "Pending" },
                    { 15, true, "ProductionStatus", null, "In Progress" },
                    { 16, true, "ProductionStatus", null, "Partially Completed" },
                    { 17, true, "ProductionStatus", null, "Completed" },
                    { 18, true, "ProductionStatus", null, "On Hold" },
                    { 19, true, "ProductionStatus", null, "Cancelled" },
                    { 20, true, "TestResult", null, "Pending" },
                    { 21, true, "TestResult", null, "Testing" },
                    { 22, true, "TestResult", null, "Passed" },
                    { 23, true, "TestResult", null, "Conditionally Passed" },
                    { 24, true, "TestResult", null, "Failed" },
                    { 25, true, "ShipmentStatus", null, "Planned" },
                    { 26, true, "ShipmentStatus", null, "Booking Confirmed" },
                    { 27, true, "ShipmentStatus", null, "Exported" },
                    { 28, true, "ShipmentStatus", null, "In Transit" },
                    { 29, true, "ShipmentStatus", null, "Delivered" },
                    { 30, true, "ShipmentStatus", null, "Cancelled" },
                    { 31, true, "POStatus", null, "Not Started" },
                    { 32, true, "POStatus", null, "Pending" },
                    { 33, true, "POStatus", null, "In Progress" },
                    { 34, true, "POStatus", null, "Partially Completed" },
                    { 35, true, "POStatus", null, "Completed" },
                    { 36, true, "POStatus", null, "Approved" },
                    { 37, true, "POStatus", null, "Conditionally Approved" },
                    { 38, true, "POStatus", null, "Rejected" },
                    { 39, true, "POStatus", null, "On Hold" },
                    { 40, true, "POStatus", null, "Closed" },
                    { 41, true, "POStatus", null, "Cancelled" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MillTests_FGPOId_FabricPOId",
                table: "MillTests",
                columns: new[] { "FGPOId", "FabricPOId" });

            migrationBuilder.CreateIndex(
                name: "IX_MillTests_LotId",
                table: "MillTests",
                column: "LotId");

            migrationBuilder.CreateIndex(
                name: "IX_MillTests_LotNumber",
                table: "MillTests",
                column: "LotNumber");

            migrationBuilder.CreateIndex(
                name: "IX_MillTests_TestResult",
                table: "MillTests",
                column: "TestResult");

            migrationBuilder.CreateIndex(
                name: "IX_MillProductions_FGPOId_FabricPOId",
                table: "MillProductions",
                columns: new[] { "FGPOId", "FabricPOId" });

            migrationBuilder.CreateIndex(
                name: "IX_MillProductions_LotId",
                table: "MillProductions",
                column: "LotId");

            migrationBuilder.CreateIndex(
                name: "IX_MillProductions_LotNumber",
                table: "MillProductions",
                column: "LotNumber");

            migrationBuilder.CreateIndex(
                name: "IX_MillProductions_Status",
                table: "MillProductions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_FabricShipments_FGPOId_FabricPOId",
                table: "FabricShipments",
                columns: new[] { "FGPOId", "FabricPOId" });

            migrationBuilder.CreateIndex(
                name: "IX_FabricShipments_LotId",
                table: "FabricShipments",
                column: "LotId");

            migrationBuilder.CreateIndex(
                name: "IX_FabricShipments_LotNumber",
                table: "FabricShipments",
                column: "LotNumber");

            migrationBuilder.CreateIndex(
                name: "IX_FabricShipments_ShipmentStatus",
                table: "FabricShipments",
                column: "ShipmentStatus");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogValues_Type_Value",
                table: "CatalogValues",
                columns: new[] { "Type", "Value" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lots_FabricPOId_FGPOId",
                table: "Lots",
                columns: new[] { "FabricPOId", "FGPOId" });

            migrationBuilder.CreateIndex(
                name: "IX_Lots_FGPOId",
                table: "Lots",
                column: "FGPOId");

            migrationBuilder.CreateIndex(
                name: "IX_Lots_LotNumber",
                table: "Lots",
                column: "LotNumber",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FabricShipments_Lots_LotId",
                table: "FabricShipments",
                column: "LotId",
                principalTable: "Lots",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_MillProductions_Lots_LotId",
                table: "MillProductions",
                column: "LotId",
                principalTable: "Lots",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_MillTests_Lots_LotId",
                table: "MillTests",
                column: "LotId",
                principalTable: "Lots",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FabricShipments_Lots_LotId",
                table: "FabricShipments");

            migrationBuilder.DropForeignKey(
                name: "FK_MillProductions_Lots_LotId",
                table: "MillProductions");

            migrationBuilder.DropForeignKey(
                name: "FK_MillTests_Lots_LotId",
                table: "MillTests");

            migrationBuilder.DropTable(
                name: "CatalogValues");

            migrationBuilder.DropTable(
                name: "Lots");

            migrationBuilder.DropIndex(
                name: "IX_MillTests_FGPOId_FabricPOId",
                table: "MillTests");

            migrationBuilder.DropIndex(
                name: "IX_MillTests_LotId",
                table: "MillTests");

            migrationBuilder.DropIndex(
                name: "IX_MillTests_LotNumber",
                table: "MillTests");

            migrationBuilder.DropIndex(
                name: "IX_MillTests_TestResult",
                table: "MillTests");

            migrationBuilder.DropIndex(
                name: "IX_MillProductions_FGPOId_FabricPOId",
                table: "MillProductions");

            migrationBuilder.DropIndex(
                name: "IX_MillProductions_LotId",
                table: "MillProductions");

            migrationBuilder.DropIndex(
                name: "IX_MillProductions_LotNumber",
                table: "MillProductions");

            migrationBuilder.DropIndex(
                name: "IX_MillProductions_Status",
                table: "MillProductions");

            migrationBuilder.DropIndex(
                name: "IX_FabricShipments_FGPOId_FabricPOId",
                table: "FabricShipments");

            migrationBuilder.DropIndex(
                name: "IX_FabricShipments_LotId",
                table: "FabricShipments");

            migrationBuilder.DropIndex(
                name: "IX_FabricShipments_LotNumber",
                table: "FabricShipments");

            migrationBuilder.DropIndex(
                name: "IX_FabricShipments_ShipmentStatus",
                table: "FabricShipments");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "LotId",
                table: "MillTests");

            migrationBuilder.DropColumn(
                name: "LotId",
                table: "MillProductions");

            migrationBuilder.DropColumn(
                name: "LotId",
                table: "FabricShipments");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "MillTests",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "MillProductions",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<decimal>(
                name: "CompletionPercentage",
                table: "MillProductions",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldComputedColumnSql: "CAST((CASE WHEN [PlannedQuantity] = 0 THEN 0 ELSE ([ProducedQuantity] / [PlannedQuantity]) * 100 END) AS decimal(18,4))");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Fgpos",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Factories",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<decimal>(
                name: "RemainingToDeliver",
                table: "FabricShipments",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldComputedColumnSql: "CAST((CASE WHEN [DeliveredToTexnicaDate] IS NULL THEN [ShippedQuantity] ELSE 0 END) AS decimal(18,4))");

            migrationBuilder.AlterColumn<decimal>(
                name: "InTransitQuantity",
                table: "FabricShipments",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldComputedColumnSql: "CAST((CASE WHEN [DeliveredToTexnicaDate] IS NULL THEN [ShippedQuantity] ELSE 0 END) AS decimal(18,4))");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "FabricShipments",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "FabricRequirements",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "FabricPOs",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Customers",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.CreateIndex(
                name: "IX_MillTests_FGPOId",
                table: "MillTests",
                column: "FGPOId");

            migrationBuilder.CreateIndex(
                name: "IX_MillProductions_FGPOId",
                table: "MillProductions",
                column: "FGPOId");

            migrationBuilder.CreateIndex(
                name: "IX_FabricShipments_FGPOId",
                table: "FabricShipments",
                column: "FGPOId");
        }
    }
}
