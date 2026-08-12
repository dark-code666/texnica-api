using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddEndlineAndPreFinalInspections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EndlineInspections",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InspectionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FGPOId = table.Column<int>(type: "int", nullable: false),
                    LotShipment = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LotSize = table.Column<int>(type: "int", nullable: false),
                    InspectionLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AqlMajor = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    AqlMinor = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    SampleSize = table.Column<int>(type: "int", nullable: false),
                    CriticalDefects = table.Column<int>(type: "int", nullable: false),
                    MajorDefects = table.Column<int>(type: "int", nullable: false),
                    MinorDefects = table.Column<int>(type: "int", nullable: false),
                    CriticalAc = table.Column<int>(type: "int", nullable: false),
                    MajorAc = table.Column<int>(type: "int", nullable: false),
                    MinorAc = table.Column<int>(type: "int", nullable: false),
                    CriticalRe = table.Column<int>(type: "int", nullable: false),
                    MajorRe = table.Column<int>(type: "int", nullable: false),
                    MinorRe = table.Column<int>(type: "int", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(450)", nullable: true, computedColumnSql: "CAST((CASE WHEN [CriticalDefects] >= [CriticalRe] OR [MajorDefects] >= [MajorRe] OR [MinorDefects] >= [MinorRe] THEN 'Failed' ELSE 'Passed' END) AS nvarchar(50))"),
                    Inspector = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Disposition = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReportLink = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EndlineInspections", x => x.ID);
                    table.ForeignKey(
                        name: "FK_EndlineInspections_Fgpos_FGPOId",
                        column: x => x.FGPOId,
                        principalTable: "Fgpos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PreFinalInspections",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InspectionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FGPOId = table.Column<int>(type: "int", nullable: false),
                    LotShipment = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LotSize = table.Column<int>(type: "int", nullable: false),
                    InspectionLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AqlMajor = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    AqlMinor = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    SampleSize = table.Column<int>(type: "int", nullable: false),
                    CriticalDefects = table.Column<int>(type: "int", nullable: false),
                    MajorDefects = table.Column<int>(type: "int", nullable: false),
                    MinorDefects = table.Column<int>(type: "int", nullable: false),
                    CriticalAc = table.Column<int>(type: "int", nullable: false),
                    MajorAc = table.Column<int>(type: "int", nullable: false),
                    MinorAc = table.Column<int>(type: "int", nullable: false),
                    CriticalRe = table.Column<int>(type: "int", nullable: false),
                    MajorRe = table.Column<int>(type: "int", nullable: false),
                    MinorRe = table.Column<int>(type: "int", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(450)", nullable: true, computedColumnSql: "CAST((CASE WHEN [CriticalDefects] >= [CriticalRe] OR [MajorDefects] >= [MajorRe] OR [MinorDefects] >= [MinorRe] THEN 'Failed' ELSE 'Passed' END) AS nvarchar(50))"),
                    Inspector = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Disposition = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReportLink = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreFinalInspections", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PreFinalInspections_Fgpos_FGPOId",
                        column: x => x.FGPOId,
                        principalTable: "Fgpos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EndlineInspections_FGPOId",
                table: "EndlineInspections",
                column: "FGPOId");

            migrationBuilder.CreateIndex(
                name: "IX_EndlineInspections_LotShipment",
                table: "EndlineInspections",
                column: "LotShipment");

            migrationBuilder.CreateIndex(
                name: "IX_EndlineInspections_Result",
                table: "EndlineInspections",
                column: "Result");

            migrationBuilder.CreateIndex(
                name: "IX_PreFinalInspections_FGPOId",
                table: "PreFinalInspections",
                column: "FGPOId");

            migrationBuilder.CreateIndex(
                name: "IX_PreFinalInspections_LotShipment",
                table: "PreFinalInspections",
                column: "LotShipment");

            migrationBuilder.CreateIndex(
                name: "IX_PreFinalInspections_Result",
                table: "PreFinalInspections",
                column: "Result");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EndlineInspections");

            migrationBuilder.DropTable(
                name: "PreFinalInspections");
        }
    }
}
