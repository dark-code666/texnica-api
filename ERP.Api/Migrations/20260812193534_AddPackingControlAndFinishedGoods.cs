using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddPackingControlAndFinishedGoods : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinishedGoods",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceiptDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FGPOId = table.Column<int>(type: "int", nullable: false),
                    PackedQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    WarehouseReceived = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ReservedForShipment = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    LoadedQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ShippedQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ReadyToShipQty = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "CAST((CASE WHEN [WarehouseReceived] - [ReservedForShipment] - [LoadedQty] - [ShippedQty] > 0 THEN [WarehouseReceived] - [ReservedForShipment] - [LoadedQty] - [ShippedQty] ELSE 0 END) AS decimal(18,4))"),
                    WarehouseBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "CAST((CASE WHEN [WarehouseReceived] - [LoadedQty] - [ShippedQty] > 0 THEN [WarehouseReceived] - [LoadedQty] - [ShippedQty] ELSE 0 END) AS decimal(18,4))"),
                    WarehouseLocation = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DataOwnerId = table.Column<int>(type: "int", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinishedGoods", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FinishedGoods_Fgpos_FGPOId",
                        column: x => x.FGPOId,
                        principalTable: "Fgpos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FinishedGoods_Users_DataOwnerId",
                        column: x => x.DataOwnerId,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PackingControls",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PackingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FGPOId = table.Column<int>(type: "int", nullable: false),
                    QcPassedQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ReceivedByPackingQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    FoldedQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    PolybaggedQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    PackedQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    FullCartons = table.Column<int>(type: "int", nullable: false),
                    PartialCartons = table.Column<int>(type: "int", nullable: false),
                    PcsPerCarton = table.Column<int>(type: "int", nullable: false),
                    ReadyToShipQty = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "CAST([PackedQty] AS decimal(18,4))"),
                    PackingVariance = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "CAST(([PackedQty] - [QcPassedQty]) AS decimal(18,4))"),
                    PendingPacking = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "CAST((CASE WHEN [QcPassedQty] - [PackedQty] > 0 THEN [QcPassedQty] - [PackedQty] ELSE 0 END) AS decimal(18,4))"),
                    OverpackedQty = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "CAST((CASE WHEN [PackedQty] - [QcPassedQty] > 0 THEN [PackedQty] - [QcPassedQty] ELSE 0 END) AS decimal(18,4))"),
                    ResponsiblePersonId = table.Column<int>(type: "int", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackingControls", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PackingControls_Fgpos_FGPOId",
                        column: x => x.FGPOId,
                        principalTable: "Fgpos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PackingControls_Users_ResponsiblePersonId",
                        column: x => x.ResponsiblePersonId,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinishedGoods_DataOwnerId",
                table: "FinishedGoods",
                column: "DataOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_FinishedGoods_FGPOId",
                table: "FinishedGoods",
                column: "FGPOId");

            migrationBuilder.CreateIndex(
                name: "IX_FinishedGoods_ReceiptDate",
                table: "FinishedGoods",
                column: "ReceiptDate");

            migrationBuilder.CreateIndex(
                name: "IX_FinishedGoods_Status",
                table: "FinishedGoods",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PackingControls_FGPOId",
                table: "PackingControls",
                column: "FGPOId");

            migrationBuilder.CreateIndex(
                name: "IX_PackingControls_PackingDate",
                table: "PackingControls",
                column: "PackingDate");

            migrationBuilder.CreateIndex(
                name: "IX_PackingControls_ResponsiblePersonId",
                table: "PackingControls",
                column: "ResponsiblePersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinishedGoods");

            migrationBuilder.DropTable(
                name: "PackingControls");
        }
    }
}
