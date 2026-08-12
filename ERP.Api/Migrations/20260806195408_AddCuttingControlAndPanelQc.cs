using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddCuttingControlAndPanelQc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CuttingControls",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CutDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FGPOId = table.Column<int>(type: "int", nullable: false),
                    Size = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FabricLot = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MarkerNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PlannedCut = table.Column<int>(type: "int", nullable: false),
                    ActualCut = table.Column<int>(type: "int", nullable: false),
                    GoodCut = table.Column<int>(type: "int", nullable: false),
                    DamagedQty = table.Column<int>(type: "int", nullable: false),
                    ReplacementCut = table.Column<int>(type: "int", nullable: false),
                    SentToSewing = table.Column<int>(type: "int", nullable: false),
                    CuttingVariance = table.Column<int>(type: "int", nullable: false, computedColumnSql: "CAST(([GoodCut] - [PlannedCut]) AS int)"),
                    PendingCut = table.Column<int>(type: "int", nullable: false, computedColumnSql: "CAST((CASE WHEN [PlannedCut] - [GoodCut] > 0 THEN [PlannedCut] - [GoodCut] ELSE 0 END) AS int)"),
                    OvercutQty = table.Column<int>(type: "int", nullable: false, computedColumnSql: "CAST((CASE WHEN [GoodCut] - [PlannedCut] > 0 THEN [GoodCut] - [PlannedCut] ELSE 0 END) AS int)"),
                    CutToSewDifference = table.Column<int>(type: "int", nullable: false, computedColumnSql: "CAST(([GoodCut] - [SentToSewing]) AS int)"),
                    ReleaseStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ResponsiblePerson = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CuttingControls", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CuttingControls_Fgpos_FGPOId",
                        column: x => x.FGPOId,
                        principalTable: "Fgpos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CuttingPanelQcs",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InspectionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FGPOId = table.Column<int>(type: "int", nullable: false),
                    Size = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FabricLot = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CutLotLay = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BundleNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SampleQty = table.Column<int>(type: "int", nullable: false),
                    PanelDefects = table.Column<int>(type: "int", nullable: false),
                    NotchesDefects = table.Column<int>(type: "int", nullable: false),
                    DrillMarkDefects = table.Column<int>(type: "int", nullable: false),
                    ShadeDefects = table.Column<int>(type: "int", nullable: false),
                    MeasurementDefects = table.Column<int>(type: "int", nullable: false),
                    TotalDefects = table.Column<int>(type: "int", nullable: false, computedColumnSql: "CAST(([PanelDefects] + [NotchesDefects] + [DrillMarkDefects] + [ShadeDefects] + [MeasurementDefects]) AS int)"),
                    DefectRatePct = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "CAST((CASE WHEN [SampleQty] = 0 THEN 0 ELSE (([PanelDefects] + [NotchesDefects] + [DrillMarkDefects] + [ShadeDefects] + [MeasurementDefects]) / CAST([SampleQty] AS decimal(18,4))) END) AS decimal(18,4))"),
                    MaxAllowed = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Result = table.Column<string>(type: "nvarchar(450)", nullable: true, computedColumnSql: "CAST((CASE WHEN [SampleQty] = 0 THEN 'Pending' WHEN (([PanelDefects] + [NotchesDefects] + [DrillMarkDefects] + [ShadeDefects] + [MeasurementDefects]) / CAST([SampleQty] AS decimal(18,4))) <= [MaxAllowed] THEN 'Passed' ELSE 'Failed' END) AS nvarchar(50))"),
                    Inspector = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CorrectiveAction = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CuttingPanelQcs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CuttingPanelQcs_Fgpos_FGPOId",
                        column: x => x.FGPOId,
                        principalTable: "Fgpos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CuttingControls_FGPOId",
                table: "CuttingControls",
                column: "FGPOId");

            migrationBuilder.CreateIndex(
                name: "IX_CuttingControls_ReleaseStatus",
                table: "CuttingControls",
                column: "ReleaseStatus");

            migrationBuilder.CreateIndex(
                name: "IX_CuttingPanelQcs_FGPOId",
                table: "CuttingPanelQcs",
                column: "FGPOId");

            migrationBuilder.CreateIndex(
                name: "IX_CuttingPanelQcs_Result",
                table: "CuttingPanelQcs",
                column: "Result");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CuttingControls");

            migrationBuilder.DropTable(
                name: "CuttingPanelQcs");
        }
    }
}
