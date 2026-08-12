using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Api.Migrations
{
    /// <inheritdoc />
    public partial class NormalizarEsquema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EndlineInspections");

            migrationBuilder.DropTable(
                name: "FinalInspections");

            migrationBuilder.DropTable(
                name: "PreFinalInspections");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "TopSamples");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "TopSamples");

            migrationBuilder.DropColumn(
                name: "DataOwner",
                table: "RollReceivings");

            migrationBuilder.DropColumn(
                name: "Supplier",
                table: "RollReceivings");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "ProductionReadiness");

            migrationBuilder.DropColumn(
                name: "ResponsibleOwner",
                table: "ProductionReadiness");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "PpSamples");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "PpSamples");

            migrationBuilder.DropColumn(
                name: "Supplier",
                table: "MillTests");

            migrationBuilder.DropColumn(
                name: "TestedBy",
                table: "MillTests");

            migrationBuilder.DropColumn(
                name: "DataOwner",
                table: "MillProductions");

            migrationBuilder.DropColumn(
                name: "FabricComponent",
                table: "MillProductions");

            migrationBuilder.DropColumn(
                name: "Supplier",
                table: "MillProductions");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "InternalTests");

            migrationBuilder.DropColumn(
                name: "Supplier",
                table: "InternalTests");

            migrationBuilder.DropColumn(
                name: "TestedBy",
                table: "InternalTests");

            migrationBuilder.DropColumn(
                name: "Inspector",
                table: "InlineQualities");

            migrationBuilder.DropColumn(
                name: "Inspector",
                table: "FourPointInspections");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Fgpos");

            migrationBuilder.DropColumn(
                name: "DataOwner",
                table: "Fgpos");

            migrationBuilder.DropColumn(
                name: "Style",
                table: "Fgpos");

            migrationBuilder.DropColumn(
                name: "DataOwner",
                table: "FabricShipments");

            migrationBuilder.DropColumn(
                name: "Supplier",
                table: "FabricShipments");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "FabricReservations");

            migrationBuilder.DropColumn(
                name: "FabricComponent",
                table: "FabricReservations");

            migrationBuilder.DropColumn(
                name: "ReservedBy",
                table: "FabricReservations");

            migrationBuilder.DropColumn(
                name: "UOM",
                table: "FabricReservations");

            migrationBuilder.DropColumn(
                name: "DataOwner",
                table: "FabricRequirements");

            migrationBuilder.DropColumn(
                name: "FabricComponent",
                table: "FabricRequirements");

            migrationBuilder.DropColumn(
                name: "DataOwner",
                table: "FabricReceivings");

            migrationBuilder.DropColumn(
                name: "ReceivedBy",
                table: "FabricReceivings");

            migrationBuilder.DropColumn(
                name: "Supplier",
                table: "FabricReceivings");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "FabricPOs");

            migrationBuilder.DropColumn(
                name: "FabricComponent",
                table: "FabricPOs");

            migrationBuilder.DropColumn(
                name: "PurchaseOwner",
                table: "FabricPOs");

            migrationBuilder.DropColumn(
                name: "Supplier",
                table: "FabricPOs");

            migrationBuilder.DropColumn(
                name: "DataOwner",
                table: "FabricInventories");

            migrationBuilder.DropColumn(
                name: "FabricComponent",
                table: "FabricInventories");

            migrationBuilder.DropColumn(
                name: "UOM",
                table: "FabricInventories");

            migrationBuilder.DropColumn(
                name: "ReleasedBy",
                table: "CuttingReleases");

            migrationBuilder.DropColumn(
                name: "ReviewedBy",
                table: "CuttingReleases");

            migrationBuilder.DropColumn(
                name: "Inspector",
                table: "CuttingPanelQcs");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "CuttingPanelQcs");

            migrationBuilder.DropColumn(
                name: "ResponsiblePerson",
                table: "CuttingControls");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "CuttingControls");

            migrationBuilder.AddColumn<int>(
                name: "ApprovedByUserId",
                table: "TopSamples",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SizeId",
                table: "TopSamples",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DataOwnerId",
                table: "RollReceivings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApprovedByUserId",
                table: "ProductionReadiness",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResponsibleOwnerId",
                table: "ProductionReadiness",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApprovedByUserId",
                table: "PpSamples",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SizeId",
                table: "PpSamples",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TestedByUserId",
                table: "MillTests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DataOwnerId",
                table: "MillProductions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApprovedByUserId",
                table: "InternalTests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TestedByUserId",
                table: "InternalTests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InspectorId",
                table: "InlineQualities",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InspectorId",
                table: "FourPointInspections",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DataOwnerId",
                table: "Fgpos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DataOwnerId",
                table: "FabricShipments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApprovedByUserId",
                table: "FabricReservations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReservedByUserId",
                table: "FabricReservations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ComponentId",
                table: "FabricRequirements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DataOwnerId",
                table: "FabricRequirements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DataOwnerId",
                table: "FabricReceivings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReceivedByUserId",
                table: "FabricReceivings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApprovedByUserId",
                table: "FabricPOs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ComponentId",
                table: "FabricPOs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PurchaseOwnerUserId",
                table: "FabricPOs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "FabricPOs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "FabricPOFgpos",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "FabricPOFgpos",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "FabricPOFgpos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DataOwnerId",
                table: "FabricInventories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReleasedByUserId",
                table: "CuttingReleases",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReviewedByUserId",
                table: "CuttingReleases",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InspectorId",
                table: "CuttingPanelQcs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SizeId",
                table: "CuttingPanelQcs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResponsiblePersonId",
                table: "CuttingControls",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SizeId",
                table: "CuttingControls",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AqlInspections",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InspectionType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
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
                    InspectorId = table.Column<int>(type: "int", nullable: true),
                    Disposition = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReportLink = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AqlInspections", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AqlInspections_Fgpos_FGPOId",
                        column: x => x.FGPOId,
                        principalTable: "Fgpos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AqlInspections_Users_InspectorId",
                        column: x => x.InspectorId,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TopSamples_ApprovedByUserId",
                table: "TopSamples",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TopSamples_SizeId",
                table: "TopSamples",
                column: "SizeId");

            migrationBuilder.CreateIndex(
                name: "IX_RollReceivings_DataOwnerId",
                table: "RollReceivings",
                column: "DataOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionReadiness_ApprovedByUserId",
                table: "ProductionReadiness",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionReadiness_ResponsibleOwnerId",
                table: "ProductionReadiness",
                column: "ResponsibleOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_PpSamples_ApprovedByUserId",
                table: "PpSamples",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PpSamples_SizeId",
                table: "PpSamples",
                column: "SizeId");

            migrationBuilder.CreateIndex(
                name: "IX_MillTests_TestedByUserId",
                table: "MillTests",
                column: "TestedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MillProductions_DataOwnerId",
                table: "MillProductions",
                column: "DataOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_InternalTests_ApprovedByUserId",
                table: "InternalTests",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_InternalTests_TestedByUserId",
                table: "InternalTests",
                column: "TestedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_InlineQualities_InspectorId",
                table: "InlineQualities",
                column: "InspectorId");

            migrationBuilder.CreateIndex(
                name: "IX_FourPointInspections_InspectorId",
                table: "FourPointInspections",
                column: "InspectorId");

            migrationBuilder.CreateIndex(
                name: "IX_Fgpos_DataOwnerId",
                table: "Fgpos",
                column: "DataOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_FabricShipments_DataOwnerId",
                table: "FabricShipments",
                column: "DataOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_FabricReservations_ApprovedByUserId",
                table: "FabricReservations",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FabricReservations_ReservedByUserId",
                table: "FabricReservations",
                column: "ReservedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FabricRequirements_ComponentId",
                table: "FabricRequirements",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_FabricRequirements_DataOwnerId",
                table: "FabricRequirements",
                column: "DataOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_FabricReceivings_DataOwnerId",
                table: "FabricReceivings",
                column: "DataOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_FabricReceivings_ReceivedByUserId",
                table: "FabricReceivings",
                column: "ReceivedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FabricPOs_ApprovedByUserId",
                table: "FabricPOs",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FabricPOs_ComponentId",
                table: "FabricPOs",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_FabricPOs_PurchaseOwnerUserId",
                table: "FabricPOs",
                column: "PurchaseOwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FabricPOs_SupplierId",
                table: "FabricPOs",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_FabricInventories_DataOwnerId",
                table: "FabricInventories",
                column: "DataOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_CuttingReleases_ReleasedByUserId",
                table: "CuttingReleases",
                column: "ReleasedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CuttingReleases_ReviewedByUserId",
                table: "CuttingReleases",
                column: "ReviewedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CuttingPanelQcs_InspectorId",
                table: "CuttingPanelQcs",
                column: "InspectorId");

            migrationBuilder.CreateIndex(
                name: "IX_CuttingPanelQcs_SizeId",
                table: "CuttingPanelQcs",
                column: "SizeId");

            migrationBuilder.CreateIndex(
                name: "IX_CuttingControls_ResponsiblePersonId",
                table: "CuttingControls",
                column: "ResponsiblePersonId");

            migrationBuilder.CreateIndex(
                name: "IX_CuttingControls_SizeId",
                table: "CuttingControls",
                column: "SizeId");

            migrationBuilder.CreateIndex(
                name: "IX_AqlInspections_FGPOId",
                table: "AqlInspections",
                column: "FGPOId");

            migrationBuilder.CreateIndex(
                name: "IX_AqlInspections_InspectionType",
                table: "AqlInspections",
                column: "InspectionType");

            migrationBuilder.CreateIndex(
                name: "IX_AqlInspections_InspectorId",
                table: "AqlInspections",
                column: "InspectorId");

            migrationBuilder.CreateIndex(
                name: "IX_AqlInspections_LotShipment",
                table: "AqlInspections",
                column: "LotShipment");

            migrationBuilder.CreateIndex(
                name: "IX_AqlInspections_Result",
                table: "AqlInspections",
                column: "Result");

            migrationBuilder.AddForeignKey(
                name: "FK_CuttingControls_Sizes_SizeId",
                table: "CuttingControls",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CuttingControls_Users_ResponsiblePersonId",
                table: "CuttingControls",
                column: "ResponsiblePersonId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CuttingPanelQcs_Sizes_SizeId",
                table: "CuttingPanelQcs",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CuttingPanelQcs_Users_InspectorId",
                table: "CuttingPanelQcs",
                column: "InspectorId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CuttingReleases_Users_ReleasedByUserId",
                table: "CuttingReleases",
                column: "ReleasedByUserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CuttingReleases_Users_ReviewedByUserId",
                table: "CuttingReleases",
                column: "ReviewedByUserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FabricInventories_Users_DataOwnerId",
                table: "FabricInventories",
                column: "DataOwnerId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FabricPOs_Components_ComponentId",
                table: "FabricPOs",
                column: "ComponentId",
                principalTable: "Components",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FabricPOs_Suppliers_SupplierId",
                table: "FabricPOs",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FabricPOs_Users_ApprovedByUserId",
                table: "FabricPOs",
                column: "ApprovedByUserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FabricPOs_Users_PurchaseOwnerUserId",
                table: "FabricPOs",
                column: "PurchaseOwnerUserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FabricReceivings_Users_DataOwnerId",
                table: "FabricReceivings",
                column: "DataOwnerId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FabricReceivings_Users_ReceivedByUserId",
                table: "FabricReceivings",
                column: "ReceivedByUserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FabricRequirements_Components_ComponentId",
                table: "FabricRequirements",
                column: "ComponentId",
                principalTable: "Components",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FabricRequirements_Users_DataOwnerId",
                table: "FabricRequirements",
                column: "DataOwnerId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FabricReservations_Users_ApprovedByUserId",
                table: "FabricReservations",
                column: "ApprovedByUserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FabricReservations_Users_ReservedByUserId",
                table: "FabricReservations",
                column: "ReservedByUserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FabricShipments_Users_DataOwnerId",
                table: "FabricShipments",
                column: "DataOwnerId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fgpos_Users_DataOwnerId",
                table: "Fgpos",
                column: "DataOwnerId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FourPointInspections_Users_InspectorId",
                table: "FourPointInspections",
                column: "InspectorId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InlineQualities_Users_InspectorId",
                table: "InlineQualities",
                column: "InspectorId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InternalTests_Users_ApprovedByUserId",
                table: "InternalTests",
                column: "ApprovedByUserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InternalTests_Users_TestedByUserId",
                table: "InternalTests",
                column: "TestedByUserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MillProductions_Users_DataOwnerId",
                table: "MillProductions",
                column: "DataOwnerId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MillTests_Users_TestedByUserId",
                table: "MillTests",
                column: "TestedByUserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PpSamples_Sizes_SizeId",
                table: "PpSamples",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PpSamples_Users_ApprovedByUserId",
                table: "PpSamples",
                column: "ApprovedByUserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionReadiness_Users_ApprovedByUserId",
                table: "ProductionReadiness",
                column: "ApprovedByUserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionReadiness_Users_ResponsibleOwnerId",
                table: "ProductionReadiness",
                column: "ResponsibleOwnerId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RollReceivings_Users_DataOwnerId",
                table: "RollReceivings",
                column: "DataOwnerId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TopSamples_Sizes_SizeId",
                table: "TopSamples",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TopSamples_Users_ApprovedByUserId",
                table: "TopSamples",
                column: "ApprovedByUserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CuttingControls_Sizes_SizeId",
                table: "CuttingControls");

            migrationBuilder.DropForeignKey(
                name: "FK_CuttingControls_Users_ResponsiblePersonId",
                table: "CuttingControls");

            migrationBuilder.DropForeignKey(
                name: "FK_CuttingPanelQcs_Sizes_SizeId",
                table: "CuttingPanelQcs");

            migrationBuilder.DropForeignKey(
                name: "FK_CuttingPanelQcs_Users_InspectorId",
                table: "CuttingPanelQcs");

            migrationBuilder.DropForeignKey(
                name: "FK_CuttingReleases_Users_ReleasedByUserId",
                table: "CuttingReleases");

            migrationBuilder.DropForeignKey(
                name: "FK_CuttingReleases_Users_ReviewedByUserId",
                table: "CuttingReleases");

            migrationBuilder.DropForeignKey(
                name: "FK_FabricInventories_Users_DataOwnerId",
                table: "FabricInventories");

            migrationBuilder.DropForeignKey(
                name: "FK_FabricPOs_Components_ComponentId",
                table: "FabricPOs");

            migrationBuilder.DropForeignKey(
                name: "FK_FabricPOs_Suppliers_SupplierId",
                table: "FabricPOs");

            migrationBuilder.DropForeignKey(
                name: "FK_FabricPOs_Users_ApprovedByUserId",
                table: "FabricPOs");

            migrationBuilder.DropForeignKey(
                name: "FK_FabricPOs_Users_PurchaseOwnerUserId",
                table: "FabricPOs");

            migrationBuilder.DropForeignKey(
                name: "FK_FabricReceivings_Users_DataOwnerId",
                table: "FabricReceivings");

            migrationBuilder.DropForeignKey(
                name: "FK_FabricReceivings_Users_ReceivedByUserId",
                table: "FabricReceivings");

            migrationBuilder.DropForeignKey(
                name: "FK_FabricRequirements_Components_ComponentId",
                table: "FabricRequirements");

            migrationBuilder.DropForeignKey(
                name: "FK_FabricRequirements_Users_DataOwnerId",
                table: "FabricRequirements");

            migrationBuilder.DropForeignKey(
                name: "FK_FabricReservations_Users_ApprovedByUserId",
                table: "FabricReservations");

            migrationBuilder.DropForeignKey(
                name: "FK_FabricReservations_Users_ReservedByUserId",
                table: "FabricReservations");

            migrationBuilder.DropForeignKey(
                name: "FK_FabricShipments_Users_DataOwnerId",
                table: "FabricShipments");

            migrationBuilder.DropForeignKey(
                name: "FK_Fgpos_Users_DataOwnerId",
                table: "Fgpos");

            migrationBuilder.DropForeignKey(
                name: "FK_FourPointInspections_Users_InspectorId",
                table: "FourPointInspections");

            migrationBuilder.DropForeignKey(
                name: "FK_InlineQualities_Users_InspectorId",
                table: "InlineQualities");

            migrationBuilder.DropForeignKey(
                name: "FK_InternalTests_Users_ApprovedByUserId",
                table: "InternalTests");

            migrationBuilder.DropForeignKey(
                name: "FK_InternalTests_Users_TestedByUserId",
                table: "InternalTests");

            migrationBuilder.DropForeignKey(
                name: "FK_MillProductions_Users_DataOwnerId",
                table: "MillProductions");

            migrationBuilder.DropForeignKey(
                name: "FK_MillTests_Users_TestedByUserId",
                table: "MillTests");

            migrationBuilder.DropForeignKey(
                name: "FK_PpSamples_Sizes_SizeId",
                table: "PpSamples");

            migrationBuilder.DropForeignKey(
                name: "FK_PpSamples_Users_ApprovedByUserId",
                table: "PpSamples");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionReadiness_Users_ApprovedByUserId",
                table: "ProductionReadiness");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionReadiness_Users_ResponsibleOwnerId",
                table: "ProductionReadiness");

            migrationBuilder.DropForeignKey(
                name: "FK_RollReceivings_Users_DataOwnerId",
                table: "RollReceivings");

            migrationBuilder.DropForeignKey(
                name: "FK_TopSamples_Sizes_SizeId",
                table: "TopSamples");

            migrationBuilder.DropForeignKey(
                name: "FK_TopSamples_Users_ApprovedByUserId",
                table: "TopSamples");

            migrationBuilder.DropTable(
                name: "AqlInspections");

            migrationBuilder.DropIndex(
                name: "IX_TopSamples_ApprovedByUserId",
                table: "TopSamples");

            migrationBuilder.DropIndex(
                name: "IX_TopSamples_SizeId",
                table: "TopSamples");

            migrationBuilder.DropIndex(
                name: "IX_RollReceivings_DataOwnerId",
                table: "RollReceivings");

            migrationBuilder.DropIndex(
                name: "IX_ProductionReadiness_ApprovedByUserId",
                table: "ProductionReadiness");

            migrationBuilder.DropIndex(
                name: "IX_ProductionReadiness_ResponsibleOwnerId",
                table: "ProductionReadiness");

            migrationBuilder.DropIndex(
                name: "IX_PpSamples_ApprovedByUserId",
                table: "PpSamples");

            migrationBuilder.DropIndex(
                name: "IX_PpSamples_SizeId",
                table: "PpSamples");

            migrationBuilder.DropIndex(
                name: "IX_MillTests_TestedByUserId",
                table: "MillTests");

            migrationBuilder.DropIndex(
                name: "IX_MillProductions_DataOwnerId",
                table: "MillProductions");

            migrationBuilder.DropIndex(
                name: "IX_InternalTests_ApprovedByUserId",
                table: "InternalTests");

            migrationBuilder.DropIndex(
                name: "IX_InternalTests_TestedByUserId",
                table: "InternalTests");

            migrationBuilder.DropIndex(
                name: "IX_InlineQualities_InspectorId",
                table: "InlineQualities");

            migrationBuilder.DropIndex(
                name: "IX_FourPointInspections_InspectorId",
                table: "FourPointInspections");

            migrationBuilder.DropIndex(
                name: "IX_Fgpos_DataOwnerId",
                table: "Fgpos");

            migrationBuilder.DropIndex(
                name: "IX_FabricShipments_DataOwnerId",
                table: "FabricShipments");

            migrationBuilder.DropIndex(
                name: "IX_FabricReservations_ApprovedByUserId",
                table: "FabricReservations");

            migrationBuilder.DropIndex(
                name: "IX_FabricReservations_ReservedByUserId",
                table: "FabricReservations");

            migrationBuilder.DropIndex(
                name: "IX_FabricRequirements_ComponentId",
                table: "FabricRequirements");

            migrationBuilder.DropIndex(
                name: "IX_FabricRequirements_DataOwnerId",
                table: "FabricRequirements");

            migrationBuilder.DropIndex(
                name: "IX_FabricReceivings_DataOwnerId",
                table: "FabricReceivings");

            migrationBuilder.DropIndex(
                name: "IX_FabricReceivings_ReceivedByUserId",
                table: "FabricReceivings");

            migrationBuilder.DropIndex(
                name: "IX_FabricPOs_ApprovedByUserId",
                table: "FabricPOs");

            migrationBuilder.DropIndex(
                name: "IX_FabricPOs_ComponentId",
                table: "FabricPOs");

            migrationBuilder.DropIndex(
                name: "IX_FabricPOs_PurchaseOwnerUserId",
                table: "FabricPOs");

            migrationBuilder.DropIndex(
                name: "IX_FabricPOs_SupplierId",
                table: "FabricPOs");

            migrationBuilder.DropIndex(
                name: "IX_FabricInventories_DataOwnerId",
                table: "FabricInventories");

            migrationBuilder.DropIndex(
                name: "IX_CuttingReleases_ReleasedByUserId",
                table: "CuttingReleases");

            migrationBuilder.DropIndex(
                name: "IX_CuttingReleases_ReviewedByUserId",
                table: "CuttingReleases");

            migrationBuilder.DropIndex(
                name: "IX_CuttingPanelQcs_InspectorId",
                table: "CuttingPanelQcs");

            migrationBuilder.DropIndex(
                name: "IX_CuttingPanelQcs_SizeId",
                table: "CuttingPanelQcs");

            migrationBuilder.DropIndex(
                name: "IX_CuttingControls_ResponsiblePersonId",
                table: "CuttingControls");

            migrationBuilder.DropIndex(
                name: "IX_CuttingControls_SizeId",
                table: "CuttingControls");

            migrationBuilder.DropColumn(
                name: "ApprovedByUserId",
                table: "TopSamples");

            migrationBuilder.DropColumn(
                name: "SizeId",
                table: "TopSamples");

            migrationBuilder.DropColumn(
                name: "DataOwnerId",
                table: "RollReceivings");

            migrationBuilder.DropColumn(
                name: "ApprovedByUserId",
                table: "ProductionReadiness");

            migrationBuilder.DropColumn(
                name: "ResponsibleOwnerId",
                table: "ProductionReadiness");

            migrationBuilder.DropColumn(
                name: "ApprovedByUserId",
                table: "PpSamples");

            migrationBuilder.DropColumn(
                name: "SizeId",
                table: "PpSamples");

            migrationBuilder.DropColumn(
                name: "TestedByUserId",
                table: "MillTests");

            migrationBuilder.DropColumn(
                name: "DataOwnerId",
                table: "MillProductions");

            migrationBuilder.DropColumn(
                name: "ApprovedByUserId",
                table: "InternalTests");

            migrationBuilder.DropColumn(
                name: "TestedByUserId",
                table: "InternalTests");

            migrationBuilder.DropColumn(
                name: "InspectorId",
                table: "InlineQualities");

            migrationBuilder.DropColumn(
                name: "InspectorId",
                table: "FourPointInspections");

            migrationBuilder.DropColumn(
                name: "DataOwnerId",
                table: "Fgpos");

            migrationBuilder.DropColumn(
                name: "DataOwnerId",
                table: "FabricShipments");

            migrationBuilder.DropColumn(
                name: "ApprovedByUserId",
                table: "FabricReservations");

            migrationBuilder.DropColumn(
                name: "ReservedByUserId",
                table: "FabricReservations");

            migrationBuilder.DropColumn(
                name: "ComponentId",
                table: "FabricRequirements");

            migrationBuilder.DropColumn(
                name: "DataOwnerId",
                table: "FabricRequirements");

            migrationBuilder.DropColumn(
                name: "DataOwnerId",
                table: "FabricReceivings");

            migrationBuilder.DropColumn(
                name: "ReceivedByUserId",
                table: "FabricReceivings");

            migrationBuilder.DropColumn(
                name: "ApprovedByUserId",
                table: "FabricPOs");

            migrationBuilder.DropColumn(
                name: "ComponentId",
                table: "FabricPOs");

            migrationBuilder.DropColumn(
                name: "PurchaseOwnerUserId",
                table: "FabricPOs");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "FabricPOs");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "FabricPOFgpos");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "FabricPOFgpos");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "FabricPOFgpos");

            migrationBuilder.DropColumn(
                name: "DataOwnerId",
                table: "FabricInventories");

            migrationBuilder.DropColumn(
                name: "ReleasedByUserId",
                table: "CuttingReleases");

            migrationBuilder.DropColumn(
                name: "ReviewedByUserId",
                table: "CuttingReleases");

            migrationBuilder.DropColumn(
                name: "InspectorId",
                table: "CuttingPanelQcs");

            migrationBuilder.DropColumn(
                name: "SizeId",
                table: "CuttingPanelQcs");

            migrationBuilder.DropColumn(
                name: "ResponsiblePersonId",
                table: "CuttingControls");

            migrationBuilder.DropColumn(
                name: "SizeId",
                table: "CuttingControls");

            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "TopSamples",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "TopSamples",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DataOwner",
                table: "RollReceivings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Supplier",
                table: "RollReceivings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "ProductionReadiness",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponsibleOwner",
                table: "ProductionReadiness",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "PpSamples",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "PpSamples",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Supplier",
                table: "MillTests",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TestedBy",
                table: "MillTests",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DataOwner",
                table: "MillProductions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FabricComponent",
                table: "MillProductions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Supplier",
                table: "MillProductions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "InternalTests",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Supplier",
                table: "InternalTests",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TestedBy",
                table: "InternalTests",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Inspector",
                table: "InlineQualities",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Inspector",
                table: "FourPointInspections",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Fgpos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DataOwner",
                table: "Fgpos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Style",
                table: "Fgpos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DataOwner",
                table: "FabricShipments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Supplier",
                table: "FabricShipments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "FabricReservations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FabricComponent",
                table: "FabricReservations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReservedBy",
                table: "FabricReservations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UOM",
                table: "FabricReservations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DataOwner",
                table: "FabricRequirements",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FabricComponent",
                table: "FabricRequirements",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DataOwner",
                table: "FabricReceivings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceivedBy",
                table: "FabricReceivings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Supplier",
                table: "FabricReceivings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "FabricPOs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FabricComponent",
                table: "FabricPOs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PurchaseOwner",
                table: "FabricPOs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Supplier",
                table: "FabricPOs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DataOwner",
                table: "FabricInventories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FabricComponent",
                table: "FabricInventories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UOM",
                table: "FabricInventories",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReleasedBy",
                table: "CuttingReleases",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReviewedBy",
                table: "CuttingReleases",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Inspector",
                table: "CuttingPanelQcs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "CuttingPanelQcs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponsiblePerson",
                table: "CuttingControls",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "CuttingControls",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EndlineInspections",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FGPOId = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    AqlMajor = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    AqlMinor = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CriticalAc = table.Column<int>(type: "int", nullable: false),
                    CriticalDefects = table.Column<int>(type: "int", nullable: false),
                    CriticalRe = table.Column<int>(type: "int", nullable: false),
                    Disposition = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    InspectionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InspectionLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Inspector = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LotShipment = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LotSize = table.Column<int>(type: "int", nullable: false),
                    MajorAc = table.Column<int>(type: "int", nullable: false),
                    MajorDefects = table.Column<int>(type: "int", nullable: false),
                    MajorRe = table.Column<int>(type: "int", nullable: false),
                    MinorAc = table.Column<int>(type: "int", nullable: false),
                    MinorDefects = table.Column<int>(type: "int", nullable: false),
                    MinorRe = table.Column<int>(type: "int", nullable: false),
                    ReportLink = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Result = table.Column<string>(type: "nvarchar(450)", nullable: true, computedColumnSql: "CAST((CASE WHEN [CriticalDefects] >= [CriticalRe] OR [MajorDefects] >= [MajorRe] OR [MinorDefects] >= [MinorRe] THEN 'Failed' ELSE 'Passed' END) AS nvarchar(50))"),
                    SampleSize = table.Column<int>(type: "int", nullable: false),
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
                name: "FinalInspections",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FGPOId = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    AqlMajor = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    AqlMinor = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CriticalAc = table.Column<int>(type: "int", nullable: false),
                    CriticalDefects = table.Column<int>(type: "int", nullable: false),
                    CriticalRe = table.Column<int>(type: "int", nullable: false),
                    Disposition = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    InspectionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InspectionLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Inspector = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LotShipment = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LotSize = table.Column<int>(type: "int", nullable: false),
                    MajorAc = table.Column<int>(type: "int", nullable: false),
                    MajorDefects = table.Column<int>(type: "int", nullable: false),
                    MajorRe = table.Column<int>(type: "int", nullable: false),
                    MinorAc = table.Column<int>(type: "int", nullable: false),
                    MinorDefects = table.Column<int>(type: "int", nullable: false),
                    MinorRe = table.Column<int>(type: "int", nullable: false),
                    ReportLink = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Result = table.Column<string>(type: "nvarchar(450)", nullable: true, computedColumnSql: "CAST((CASE WHEN [CriticalDefects] >= [CriticalRe] OR [MajorDefects] >= [MajorRe] OR [MinorDefects] >= [MinorRe] THEN 'Failed' ELSE 'Passed' END) AS nvarchar(50))"),
                    SampleSize = table.Column<int>(type: "int", nullable: false),
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
                name: "PreFinalInspections",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FGPOId = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    AqlMajor = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    AqlMinor = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CriticalAc = table.Column<int>(type: "int", nullable: false),
                    CriticalDefects = table.Column<int>(type: "int", nullable: false),
                    CriticalRe = table.Column<int>(type: "int", nullable: false),
                    Disposition = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    InspectionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InspectionLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Inspector = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LotShipment = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LotSize = table.Column<int>(type: "int", nullable: false),
                    MajorAc = table.Column<int>(type: "int", nullable: false),
                    MajorDefects = table.Column<int>(type: "int", nullable: false),
                    MajorRe = table.Column<int>(type: "int", nullable: false),
                    MinorAc = table.Column<int>(type: "int", nullable: false),
                    MinorDefects = table.Column<int>(type: "int", nullable: false),
                    MinorRe = table.Column<int>(type: "int", nullable: false),
                    ReportLink = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Result = table.Column<string>(type: "nvarchar(450)", nullable: true, computedColumnSql: "CAST((CASE WHEN [CriticalDefects] >= [CriticalRe] OR [MajorDefects] >= [MajorRe] OR [MinorDefects] >= [MinorRe] THEN 'Failed' ELSE 'Passed' END) AS nvarchar(50))"),
                    SampleSize = table.Column<int>(type: "int", nullable: false),
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
    }
}
