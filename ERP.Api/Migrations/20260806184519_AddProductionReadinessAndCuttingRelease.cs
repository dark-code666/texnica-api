using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddProductionReadinessAndCuttingRelease : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CuttingReleases",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReleaseNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FGPOId = table.Column<int>(type: "int", nullable: false),
                    FabricLot = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ApprovedCutQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ApprovedWidth = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    MarkerNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ApprovedYield = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    PrrResult = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReleasedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReviewedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Exception = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Conditions = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ReleaseStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CuttingReleases", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CuttingReleases_Fgpos_FGPOId",
                        column: x => x.FGPOId,
                        principalTable: "Fgpos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductionReadiness",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReviewDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FGPOId = table.Column<int>(type: "int", nullable: false),
                    PoConfirmed = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TechPackCurrent = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FabricApproved = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TrimsApproved = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TrimsAvailable = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PpSampleApproved = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PatternApproved = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MarkerApproved = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FabricWidthConfirmed = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ShrinkageApproved = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TorqueApproved = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    QualityStandardReady = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LinePlanned = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OverallResult = table.Column<string>(type: "nvarchar(450)", nullable: true, computedColumnSql: "CAST((CASE WHEN [PoConfirmed]='Not Ready' OR [TechPackCurrent]='Not Ready' OR [FabricApproved]='Not Ready' OR [TrimsApproved]='Not Ready' OR [TrimsAvailable]='Not Ready' OR [PpSampleApproved]='Not Ready' OR [PatternApproved]='Not Ready' OR [MarkerApproved]='Not Ready' OR [FabricWidthConfirmed]='Not Ready' OR [ShrinkageApproved]='Not Ready' OR [TorqueApproved]='Not Ready' OR [QualityStandardReady]='Not Ready' OR [LinePlanned]='Not Ready' THEN 'Blocked' WHEN [PoConfirmed]='Pending' OR [TechPackCurrent]='Pending' OR [FabricApproved]='Pending' OR [TrimsApproved]='Pending' OR [TrimsAvailable]='Pending' OR [PpSampleApproved]='Pending' OR [PatternApproved]='Pending' OR [MarkerApproved]='Pending' OR [FabricWidthConfirmed]='Pending' OR [ShrinkageApproved]='Pending' OR [TorqueApproved]='Pending' OR [QualityStandardReady]='Pending' OR [LinePlanned]='Pending' THEN 'Not Ready' WHEN [PoConfirmed]='Exception Approved' OR [TechPackCurrent]='Exception Approved' OR [FabricApproved]='Exception Approved' OR [TrimsApproved]='Exception Approved' OR [TrimsAvailable]='Exception Approved' OR [PpSampleApproved]='Exception Approved' OR [PatternApproved]='Exception Approved' OR [MarkerApproved]='Exception Approved' OR [FabricWidthConfirmed]='Exception Approved' OR [ShrinkageApproved]='Exception Approved' OR [TorqueApproved]='Exception Approved' OR [QualityStandardReady]='Exception Approved' OR [LinePlanned]='Exception Approved' THEN 'Ready with Conditions' ELSE 'Ready' END) AS nvarchar(50))"),
                    OpenConditions = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ResponsibleOwner = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionReadiness", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductionReadiness_Fgpos_FGPOId",
                        column: x => x.FGPOId,
                        principalTable: "Fgpos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CuttingReleases_FGPOId",
                table: "CuttingReleases",
                column: "FGPOId");

            migrationBuilder.CreateIndex(
                name: "IX_CuttingReleases_ReleaseNumber",
                table: "CuttingReleases",
                column: "ReleaseNumber",
                unique: true,
                filter: "[ReleaseNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CuttingReleases_ReleaseStatus",
                table: "CuttingReleases",
                column: "ReleaseStatus");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionReadiness_FGPOId",
                table: "ProductionReadiness",
                column: "FGPOId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionReadiness_OverallResult",
                table: "ProductionReadiness",
                column: "OverallResult");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CuttingReleases");

            migrationBuilder.DropTable(
                name: "ProductionReadiness");
        }
    }
}
