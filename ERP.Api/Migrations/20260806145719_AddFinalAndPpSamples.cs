using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddFinalAndPpSamples : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinalInspections",
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
                    table.PrimaryKey("PK_FinalInspections", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FinalInspections_Fgpos_FGPOId",
                        column: x => x.FGPOId,
                        principalTable: "Fgpos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PpSamples",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FGPOId = table.Column<int>(type: "int", nullable: false),
                    Size = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SampleVersion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FabricLot = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TrimVersion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PreparationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubmissionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MeasurementResult = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ConstructionResult = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FitResult = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FabricResult = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TrimResult = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LabelResult = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InternalReview = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustomerReview = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustomerComments = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
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
                    table.PrimaryKey("PK_PpSamples", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PpSamples_Fgpos_FGPOId",
                        column: x => x.FGPOId,
                        principalTable: "Fgpos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinalInspections_FGPOId",
                table: "FinalInspections",
                column: "FGPOId");

            migrationBuilder.CreateIndex(
                name: "IX_FinalInspections_LotShipment",
                table: "FinalInspections",
                column: "LotShipment");

            migrationBuilder.CreateIndex(
                name: "IX_FinalInspections_Result",
                table: "FinalInspections",
                column: "Result");

            migrationBuilder.CreateIndex(
                name: "IX_PpSamples_FGPOId",
                table: "PpSamples",
                column: "FGPOId");

            migrationBuilder.CreateIndex(
                name: "IX_PpSamples_SampleVersion",
                table: "PpSamples",
                column: "SampleVersion");

            migrationBuilder.CreateIndex(
                name: "IX_PpSamples_Status",
                table: "PpSamples",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinalInspections");

            migrationBuilder.DropTable(
                name: "PpSamples");
        }
    }
}
