using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddTopSample : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TopSamples",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FGPOId = table.Column<int>(type: "int", nullable: false),
                    Size = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProductionLine = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FabricLot = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CutLotBundle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TrimVersion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ThreadLot = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TopQty = table.Column<int>(type: "int", nullable: false),
                    ProductionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MeasurementResult = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ConstructionResult = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WorkmanshipResult = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LabelResult = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PackingResult = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InternalReview = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustomerReview = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CorrectiveAction = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DocumentLink = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PhotoLink = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopSamples", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TopSamples_Fgpos_FGPOId",
                        column: x => x.FGPOId,
                        principalTable: "Fgpos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TopSamples_FGPOId",
                table: "TopSamples",
                column: "FGPOId");

            migrationBuilder.CreateIndex(
                name: "IX_TopSamples_ProductionLine",
                table: "TopSamples",
                column: "ProductionLine");

            migrationBuilder.CreateIndex(
                name: "IX_TopSamples_Status",
                table: "TopSamples",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TopSamples");
        }
    }
}
