using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddFabricReceiving : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FabricReceivings",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceivingNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReceivingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShipmentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FabricPOId = table.Column<int>(type: "int", nullable: false),
                    FGPOId = table.Column<int>(type: "int", nullable: false),
                    Supplier = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PackingListQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ActualReceivedQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ReceivingVariance = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "CAST(([ActualReceivedQty] - [PackingListQty]) AS decimal(18,4))"),
                    ReceivingShortage = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "CAST((CASE WHEN [PackingListQty] > [ActualReceivedQty] THEN [PackingListQty] - [ActualReceivedQty] ELSE 0 END) AS decimal(18,4))"),
                    ReceivingOverQty = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "CAST((CASE WHEN [ActualReceivedQty] > [PackingListQty] THEN [ActualReceivedQty] - [PackingListQty] ELSE 0 END) AS decimal(18,4))"),
                    ExpectedRolls = table.Column<int>(type: "int", nullable: false),
                    ReceivedRolls = table.Column<int>(type: "int", nullable: false),
                    MissingRolls = table.Column<int>(type: "int", nullable: false, computedColumnSql: "CAST((CASE WHEN [ExpectedRolls] > [ReceivedRolls] THEN [ExpectedRolls] - [ReceivedRolls] ELSE 0 END) AS int)"),
                    ReceivingStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WarehouseLocation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReceivedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DataOwner = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FabricReceivings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FabricReceivings_FabricPOs_FabricPOId",
                        column: x => x.FabricPOId,
                        principalTable: "FabricPOs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FabricReceivings_Fgpos_FGPOId",
                        column: x => x.FGPOId,
                        principalTable: "Fgpos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FabricReceivings_FabricPOId",
                table: "FabricReceivings",
                column: "FabricPOId");

            migrationBuilder.CreateIndex(
                name: "IX_FabricReceivings_FGPOId_FabricPOId",
                table: "FabricReceivings",
                columns: new[] { "FGPOId", "FabricPOId" });

            migrationBuilder.CreateIndex(
                name: "IX_FabricReceivings_ReceivingNumber",
                table: "FabricReceivings",
                column: "ReceivingNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FabricReceivings_ReceivingStatus",
                table: "FabricReceivings",
                column: "ReceivingStatus");

            migrationBuilder.CreateIndex(
                name: "IX_FabricReceivings_ShipmentNumber",
                table: "FabricReceivings",
                column: "ShipmentNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FabricReceivings");
        }
    }
}
