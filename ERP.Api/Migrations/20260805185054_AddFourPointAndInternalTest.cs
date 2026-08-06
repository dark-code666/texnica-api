using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddFourPointAndInternalTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FourPointInspections",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InspectionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReceivingId = table.Column<int>(type: "int", nullable: true),
                    ReceivingNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FabricPOId = table.Column<int>(type: "int", nullable: false),
                    FGPOId = table.Column<int>(type: "int", nullable: false),
                    LotNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LotId = table.Column<int>(type: "int", nullable: true),
                    RollNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Width = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    InspectedLength = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Points1 = table.Column<int>(type: "int", nullable: false),
                    Points2 = table.Column<int>(type: "int", nullable: false),
                    Points3 = table.Column<int>(type: "int", nullable: false),
                    Points4 = table.Column<int>(type: "int", nullable: false),
                    TotalPoints = table.Column<int>(type: "int", nullable: false, computedColumnSql: "CAST(([Points1] + (2 * [Points2]) + (3 * [Points3]) + (4 * [Points4])) AS int)"),
                    PointsPer100SqYd = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "CAST((CASE WHEN [Width] = 0 OR [InspectedLength] = 0 THEN 0 ELSE (([Points1] + (2 * [Points2]) + (3 * [Points3]) + (4 * [Points4])) * 3600.0) / ([Width] * [InspectedLength]) END) AS decimal(18,4))"),
                    MaxAllowed = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    AcceptedQty = table.Column<int>(type: "int", nullable: false),
                    RejectedQty = table.Column<int>(type: "int", nullable: false),
                    HoldQty = table.Column<int>(type: "int", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Inspector = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReportLink = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FourPointInspections", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FourPointInspections_FabricPOs_FabricPOId",
                        column: x => x.FabricPOId,
                        principalTable: "FabricPOs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FourPointInspections_FabricReceivings_ReceivingId",
                        column: x => x.ReceivingId,
                        principalTable: "FabricReceivings",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_FourPointInspections_Fgpos_FGPOId",
                        column: x => x.FGPOId,
                        principalTable: "Fgpos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FourPointInspections_Lots_LotId",
                        column: x => x.LotId,
                        principalTable: "Lots",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "InternalTests",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FabricPOId = table.Column<int>(type: "int", nullable: false),
                    FGPOId = table.Column<int>(type: "int", nullable: false),
                    Supplier = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LotNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LotId = table.Column<int>(type: "int", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ActualWidth = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    SpecimenAreaCm2 = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    WeightBeforeG = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    WeightAfterG = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TargetGSM = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    GsmBefore = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "CAST((CASE WHEN [SpecimenAreaCm2] = 0 THEN 0 ELSE ([WeightBeforeG] / ([SpecimenAreaCm2] / 10000.0)) END) AS decimal(18,4))"),
                    GsmAfter = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "CAST((CASE WHEN [SpecimenAreaCm2] = 0 THEN 0 ELSE ([WeightAfterG] / ([SpecimenAreaCm2] / 10000.0)) END) AS decimal(18,4))"),
                    GsmVariancePct = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "CAST((CASE WHEN [TargetGSM] = 0 OR [SpecimenAreaCm2] = 0 THEN 0 ELSE ((([WeightAfterG] / ([SpecimenAreaCm2] / 10000.0)) - [TargetGSM]) / [TargetGSM]) * 100 END) AS decimal(18,4))"),
                    LengthBefore = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    LengthAfter = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    LengthShrinkagePct = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "CAST((CASE WHEN [LengthBefore] = 0 THEN 0 ELSE (([LengthBefore] - [LengthAfter]) / [LengthBefore]) * 100 END) AS decimal(18,4))"),
                    WidthBefore = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    WidthAfter = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    WidthShrinkagePct = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "CAST((CASE WHEN [WidthBefore] = 0 THEN 0 ELSE (([WidthBefore] - [WidthAfter]) / [WidthBefore]) * 100 END) AS decimal(18,4))"),
                    TorquePct = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    BowingPct = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    SkewingPct = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ShadeResult = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WashAppearance = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    HandFeel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TestResult = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TestedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReportLink = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalTests", x => x.ID);
                    table.ForeignKey(
                        name: "FK_InternalTests_FabricPOs_FabricPOId",
                        column: x => x.FabricPOId,
                        principalTable: "FabricPOs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InternalTests_Fgpos_FGPOId",
                        column: x => x.FGPOId,
                        principalTable: "Fgpos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InternalTests_Lots_LotId",
                        column: x => x.LotId,
                        principalTable: "Lots",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FourPointInspections_FabricPOId",
                table: "FourPointInspections",
                column: "FabricPOId");

            migrationBuilder.CreateIndex(
                name: "IX_FourPointInspections_FGPOId_FabricPOId",
                table: "FourPointInspections",
                columns: new[] { "FGPOId", "FabricPOId" });

            migrationBuilder.CreateIndex(
                name: "IX_FourPointInspections_LotId",
                table: "FourPointInspections",
                column: "LotId");

            migrationBuilder.CreateIndex(
                name: "IX_FourPointInspections_ReceivingId",
                table: "FourPointInspections",
                column: "ReceivingId");

            migrationBuilder.CreateIndex(
                name: "IX_FourPointInspections_Result",
                table: "FourPointInspections",
                column: "Result");

            migrationBuilder.CreateIndex(
                name: "IX_FourPointInspections_RollNumber",
                table: "FourPointInspections",
                column: "RollNumber");

            migrationBuilder.CreateIndex(
                name: "IX_InternalTests_FabricPOId",
                table: "InternalTests",
                column: "FabricPOId");

            migrationBuilder.CreateIndex(
                name: "IX_InternalTests_FGPOId_FabricPOId",
                table: "InternalTests",
                columns: new[] { "FGPOId", "FabricPOId" });

            migrationBuilder.CreateIndex(
                name: "IX_InternalTests_LotId",
                table: "InternalTests",
                column: "LotId");

            migrationBuilder.CreateIndex(
                name: "IX_InternalTests_LotNumber",
                table: "InternalTests",
                column: "LotNumber");

            migrationBuilder.CreateIndex(
                name: "IX_InternalTests_TestResult",
                table: "InternalTests",
                column: "TestResult");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FourPointInspections");

            migrationBuilder.DropTable(
                name: "InternalTests");
        }
    }
}
