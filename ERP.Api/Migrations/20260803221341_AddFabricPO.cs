using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddFabricPO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FabricPOs",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FabricPONumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Supplier = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FabricMill = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FabricComponent = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Style = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Color = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OrderedQuantity = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    UOM = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    POAmount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequiredCompletion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlannedExport = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PlannedArrival = table.Column<DateTime>(type: "datetime2", nullable: true),
                    POStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PurchaseOwner = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DataOwner = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FabricPOs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FabricPOFgpos",
                columns: table => new
                {
                    FabricPOId = table.Column<int>(type: "int", nullable: false),
                    FGPOId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FabricPOFgpos", x => new { x.FabricPOId, x.FGPOId });
                    table.ForeignKey(
                        name: "FK_FabricPOFgpos_FabricPOs_FabricPOId",
                        column: x => x.FabricPOId,
                        principalTable: "FabricPOs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FabricPOFgpos_Fgpos_FGPOId",
                        column: x => x.FGPOId,
                        principalTable: "Fgpos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FabricPOFgpos_FGPOId",
                table: "FabricPOFgpos",
                column: "FGPOId");

            migrationBuilder.CreateIndex(
                name: "IX_FabricPOs_FabricPONumber",
                table: "FabricPOs",
                column: "FabricPONumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FabricPOFgpos");

            migrationBuilder.DropTable(
                name: "FabricPOs");
        }
    }
}
