using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddShipmentControl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShipmentControls",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShipmentNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PlannedLoadingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualLoadingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ETD = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ETA = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FGPOId = table.Column<int>(type: "int", nullable: false),
                    PlannedQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ActualLoadedQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    InTransitQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    CustomerReceivedQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TotalShippedQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ShipmentVariance = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "CAST(([TotalShippedQty] - [PlannedQty]) AS decimal(18,4))"),
                    PendingToShip = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "CAST((CASE WHEN [PlannedQty] - [TotalShippedQty] > 0 THEN [PlannedQty] - [TotalShippedQty] ELSE 0 END) AS decimal(18,4))"),
                    OvershipmentQty = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "CAST((CASE WHEN [TotalShippedQty] - [PlannedQty] > 0 THEN [TotalShippedQty] - [PlannedQty] ELSE 0 END) AS decimal(18,4))"),
                    ContainerType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContainerNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BookingNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Destination = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ShipmentStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PackingList = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LoadPlan = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DataOwnerId = table.Column<int>(type: "int", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipmentControls", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ShipmentControls_Fgpos_FGPOId",
                        column: x => x.FGPOId,
                        principalTable: "Fgpos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShipmentControls_Users_DataOwnerId",
                        column: x => x.DataOwnerId,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentControls_DataOwnerId",
                table: "ShipmentControls",
                column: "DataOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentControls_FGPOId",
                table: "ShipmentControls",
                column: "FGPOId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentControls_ShipmentNumber",
                table: "ShipmentControls",
                column: "ShipmentNumber");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentControls_ShipmentStatus",
                table: "ShipmentControls",
                column: "ShipmentStatus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShipmentControls");
        }
    }
}
