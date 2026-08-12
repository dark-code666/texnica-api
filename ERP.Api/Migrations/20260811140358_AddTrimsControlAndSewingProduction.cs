using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddTrimsControlAndSewingProduction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SewingProductions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Shift = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Line = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FGPOId = table.Column<int>(type: "int", nullable: false),
                    Size = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SewingInput = table.Column<int>(type: "int", nullable: false),
                    DailyTarget = table.Column<int>(type: "int", nullable: false),
                    DailyOutput = table.Column<int>(type: "int", nullable: false),
                    CumulativeOutput = table.Column<int>(type: "int", nullable: false),
                    Wip = table.Column<int>(type: "int", nullable: false),
                    Rework = table.Column<int>(type: "int", nullable: false),
                    Reject = table.Column<int>(type: "int", nullable: false),
                    DowntimeMinutes = table.Column<int>(type: "int", nullable: false),
                    TargetAchievementPct = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "CAST((CASE WHEN [DailyTarget] = 0 THEN 0 ELSE ([DailyOutput] / CAST([DailyTarget] AS decimal(18,4))) END) AS decimal(18,4))"),
                    SewingVariance = table.Column<int>(type: "int", nullable: false, computedColumnSql: "CAST(([CumulativeOutput] - [SewingInput]) AS int)"),
                    PendingSewing = table.Column<int>(type: "int", nullable: false, computedColumnSql: "CAST((CASE WHEN [SewingInput] - [CumulativeOutput] > 0 THEN [SewingInput] - [CumulativeOutput] ELSE 0 END) AS int)"),
                    Overproduction = table.Column<int>(type: "int", nullable: false, computedColumnSql: "CAST((CASE WHEN [CumulativeOutput] - [SewingInput] > 0 THEN [CumulativeOutput] - [SewingInput] ELSE 0 END) AS int)"),
                    TopStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Supervisor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SewingProductions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SewingProductions_Fgpos_FGPOId",
                        column: x => x.FGPOId,
                        principalTable: "Fgpos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrimsControls",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FGPOId = table.Column<int>(type: "int", nullable: false),
                    TrimType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Supplier = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Uom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ConsumptionPerGarment = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    RequiredQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    OrderedQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ReceivedQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ApprovedQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    RejectedQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ReservedQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    IssuedQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    AvailableQty = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "CAST((CASE WHEN [ApprovedQty] - [ReservedQty] - [IssuedQty] > 0 THEN [ApprovedQty] - [ReservedQty] - [IssuedQty] ELSE 0 END) AS decimal(18,4))"),
                    ShortageQty = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "CAST((CASE WHEN [RequiredQty] - [ApprovedQty] > 0 THEN [RequiredQty] - [ApprovedQty] ELSE 0 END) AS decimal(18,4))"),
                    AvailabilityStatus = table.Column<string>(type: "nvarchar(450)", nullable: true, computedColumnSql: "CAST((CASE WHEN [RequiredQty] - [ApprovedQty] > 0 THEN 'Shortage' WHEN (CASE WHEN [ApprovedQty] - [ReservedQty] - [IssuedQty] > 0 THEN [ApprovedQty] - [ReservedQty] - [IssuedQty] ELSE 0 END) >= [RequiredQty] THEN 'Ready' WHEN (CASE WHEN [ApprovedQty] - [ReservedQty] - [IssuedQty] > 0 THEN [ApprovedQty] - [ReservedQty] - [IssuedQty] ELSE 0 END) > 0 THEN 'Partially Ready' ELSE 'Pending' END) AS nvarchar(50))"),
                    Eta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DevelopmentStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ApprovalStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DataOwner = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrimsControls", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TrimsControls_Fgpos_FGPOId",
                        column: x => x.FGPOId,
                        principalTable: "Fgpos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SewingProductions_FGPOId",
                table: "SewingProductions",
                column: "FGPOId");

            migrationBuilder.CreateIndex(
                name: "IX_SewingProductions_Line",
                table: "SewingProductions",
                column: "Line");

            migrationBuilder.CreateIndex(
                name: "IX_TrimsControls_AvailabilityStatus",
                table: "TrimsControls",
                column: "AvailabilityStatus");

            migrationBuilder.CreateIndex(
                name: "IX_TrimsControls_FGPOId",
                table: "TrimsControls",
                column: "FGPOId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SewingProductions");

            migrationBuilder.DropTable(
                name: "TrimsControls");
        }
    }
}
