using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddShadeMatchAndInlineQuality : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InlineQualities",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InspectionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Line = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FGPOId = table.Column<int>(type: "int", nullable: false),
                    Operation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Operator = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CheckedQty = table.Column<int>(type: "int", nullable: false),
                    CriticalDefects = table.Column<int>(type: "int", nullable: false),
                    MajorDefects = table.Column<int>(type: "int", nullable: false),
                    MinorDefects = table.Column<int>(type: "int", nullable: false),
                    TotalDefects = table.Column<int>(type: "int", nullable: false, computedColumnSql: "CAST(([CriticalDefects] + [MajorDefects] + [MinorDefects]) AS int)"),
                    DhuPct = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "CAST((CASE WHEN [CheckedQty] = 0 THEN 0 ELSE (([CriticalDefects] + [MajorDefects] + [MinorDefects]) / CAST([CheckedQty] AS decimal(18,4))) * 100 END) AS decimal(18,4))"),
                    DefectivePieces = table.Column<int>(type: "int", nullable: false),
                    DefectiveRatePct = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "CAST((CASE WHEN [CheckedQty] = 0 THEN 0 ELSE ([DefectivePieces] / CAST([CheckedQty] AS decimal(18,4))) * 100 END) AS decimal(18,4))"),
                    MaxAllowed = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Result = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Inspector = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ImmediateCorrection = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RootCause = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InlineQualities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_InlineQualities_Fgpos_FGPOId",
                        column: x => x.FGPOId,
                        principalTable: "Fgpos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShadeMatches",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReviewDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FGPOId = table.Column<int>(type: "int", nullable: false),
                    BodyFabricLot = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RibLot = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ShoulderTapeLot = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BodyShadeGroup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RibShadeGroup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TapeShadeGroup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BodyVsRib = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BodyVsTape = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LightSource = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BeforeWashResult = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AfterWashResult = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OverallResult = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReportLink = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShadeMatches", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ShadeMatches_Fgpos_FGPOId",
                        column: x => x.FGPOId,
                        principalTable: "Fgpos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InlineQualities_FGPOId",
                table: "InlineQualities",
                column: "FGPOId");

            migrationBuilder.CreateIndex(
                name: "IX_InlineQualities_Line",
                table: "InlineQualities",
                column: "Line");

            migrationBuilder.CreateIndex(
                name: "IX_InlineQualities_Result",
                table: "InlineQualities",
                column: "Result");

            migrationBuilder.CreateIndex(
                name: "IX_ShadeMatches_BodyFabricLot",
                table: "ShadeMatches",
                column: "BodyFabricLot");

            migrationBuilder.CreateIndex(
                name: "IX_ShadeMatches_FGPOId",
                table: "ShadeMatches",
                column: "FGPOId");

            migrationBuilder.CreateIndex(
                name: "IX_ShadeMatches_OverallResult",
                table: "ShadeMatches",
                column: "OverallResult");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InlineQualities");

            migrationBuilder.DropTable(
                name: "ShadeMatches");
        }
    }
}
