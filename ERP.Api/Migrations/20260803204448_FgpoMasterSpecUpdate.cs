using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Api.Migrations
{
    /// <inheritdoc />
    public partial class FgpoMasterSpecUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fgpos_Factories_FactoryId",
                table: "Fgpos");

            migrationBuilder.DropIndex(
                name: "IX_Fgpos_FactoryId",
                table: "Fgpos");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Fgpos");

            migrationBuilder.DropColumn(
                name: "Buyer",
                table: "Fgpos");

            migrationBuilder.DropColumn(
                name: "FactoryId",
                table: "Fgpos");

            migrationBuilder.DropColumn(
                name: "Season",
                table: "Fgpos");

            migrationBuilder.DropColumn(
                name: "SizeRange",
                table: "Fgpos");

            migrationBuilder.DropColumn(
                name: "StyleDescription",
                table: "Fgpos");

            migrationBuilder.DropColumn(
                name: "BalanceQuantity",
                table: "FabricRequirements");

            migrationBuilder.DropColumn(
                name: "ColorCode",
                table: "FabricRequirements");

            migrationBuilder.DropColumn(
                name: "FabricCode",
                table: "FabricRequirements");

            migrationBuilder.DropColumn(
                name: "LineNumber",
                table: "FabricRequirements");

            migrationBuilder.RenameColumn(
                name: "PurchaseOrder",
                table: "Fgpos",
                newName: "DataOwner");

            migrationBuilder.RenameColumn(
                name: "Width",
                table: "FabricRequirements",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "FabricRequirements",
                newName: "RequiredWidth");

            migrationBuilder.RenameColumn(
                name: "WastePercentage",
                table: "FabricRequirements",
                newName: "OrderQuantity");

            migrationBuilder.RenameColumn(
                name: "UnitOfMeasure",
                table: "FabricRequirements",
                newName: "UOM");

            migrationBuilder.RenameColumn(
                name: "Supplier",
                table: "FabricRequirements",
                newName: "Style");

            migrationBuilder.RenameColumn(
                name: "ReservedQuantity",
                table: "FabricRequirements",
                newName: "NetPurchaseRequirement");

            migrationBuilder.RenameColumn(
                name: "ReceivedQuantity",
                table: "FabricRequirements",
                newName: "GSM");

            migrationBuilder.RenameColumn(
                name: "OrderedQuantity",
                table: "FabricRequirements",
                newName: "AvailableInventory");

            migrationBuilder.RenameColumn(
                name: "NetRequirement",
                table: "FabricRequirements",
                newName: "ApprovedYield");

            migrationBuilder.RenameColumn(
                name: "IssuedQuantity",
                table: "FabricRequirements",
                newName: "AllowanceQty");

            migrationBuilder.RenameColumn(
                name: "FabricType",
                table: "FabricRequirements",
                newName: "FabricComponent");

            migrationBuilder.RenameColumn(
                name: "FabricName",
                table: "FabricRequirements",
                newName: "DataOwner");

            migrationBuilder.RenameColumn(
                name: "ConsumptionPerGarment",
                table: "FabricRequirements",
                newName: "AllowancePercentage");

            migrationBuilder.AddColumn<decimal>(
                name: "InTransitQty",
                table: "Fgpos",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OverproductionQty",
                table: "Fgpos",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OvershipmentQty",
                table: "Fgpos",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PendingProduction",
                table: "Fgpos",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PendingToShip",
                table: "Fgpos",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ProducedQty",
                table: "Fgpos",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ProductionVariance",
                table: "Fgpos",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ReceivedQty",
                table: "Fgpos",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ShipmentVariance",
                table: "Fgpos",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalShippedQty",
                table: "Fgpos",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "FabricDescription",
                table: "FabricRequirements",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequiredDate",
                table: "FabricRequirements",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InTransitQty",
                table: "Fgpos");

            migrationBuilder.DropColumn(
                name: "OverproductionQty",
                table: "Fgpos");

            migrationBuilder.DropColumn(
                name: "OvershipmentQty",
                table: "Fgpos");

            migrationBuilder.DropColumn(
                name: "PendingProduction",
                table: "Fgpos");

            migrationBuilder.DropColumn(
                name: "PendingToShip",
                table: "Fgpos");

            migrationBuilder.DropColumn(
                name: "ProducedQty",
                table: "Fgpos");

            migrationBuilder.DropColumn(
                name: "ProductionVariance",
                table: "Fgpos");

            migrationBuilder.DropColumn(
                name: "ReceivedQty",
                table: "Fgpos");

            migrationBuilder.DropColumn(
                name: "ShipmentVariance",
                table: "Fgpos");

            migrationBuilder.DropColumn(
                name: "TotalShippedQty",
                table: "Fgpos");

            migrationBuilder.DropColumn(
                name: "FabricDescription",
                table: "FabricRequirements");

            migrationBuilder.DropColumn(
                name: "RequiredDate",
                table: "FabricRequirements");

            migrationBuilder.RenameColumn(
                name: "DataOwner",
                table: "Fgpos",
                newName: "PurchaseOrder");

            migrationBuilder.RenameColumn(
                name: "UOM",
                table: "FabricRequirements",
                newName: "UnitOfMeasure");

            migrationBuilder.RenameColumn(
                name: "Style",
                table: "FabricRequirements",
                newName: "Supplier");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "FabricRequirements",
                newName: "Width");

            migrationBuilder.RenameColumn(
                name: "RequiredWidth",
                table: "FabricRequirements",
                newName: "Weight");

            migrationBuilder.RenameColumn(
                name: "OrderQuantity",
                table: "FabricRequirements",
                newName: "WastePercentage");

            migrationBuilder.RenameColumn(
                name: "NetPurchaseRequirement",
                table: "FabricRequirements",
                newName: "ReservedQuantity");

            migrationBuilder.RenameColumn(
                name: "GSM",
                table: "FabricRequirements",
                newName: "ReceivedQuantity");

            migrationBuilder.RenameColumn(
                name: "FabricComponent",
                table: "FabricRequirements",
                newName: "FabricType");

            migrationBuilder.RenameColumn(
                name: "DataOwner",
                table: "FabricRequirements",
                newName: "FabricName");

            migrationBuilder.RenameColumn(
                name: "AvailableInventory",
                table: "FabricRequirements",
                newName: "OrderedQuantity");

            migrationBuilder.RenameColumn(
                name: "ApprovedYield",
                table: "FabricRequirements",
                newName: "NetRequirement");

            migrationBuilder.RenameColumn(
                name: "AllowanceQty",
                table: "FabricRequirements",
                newName: "IssuedQuantity");

            migrationBuilder.RenameColumn(
                name: "AllowancePercentage",
                table: "FabricRequirements",
                newName: "ConsumptionPerGarment");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Fgpos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Buyer",
                table: "Fgpos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FactoryId",
                table: "Fgpos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Season",
                table: "Fgpos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SizeRange",
                table: "Fgpos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StyleDescription",
                table: "Fgpos",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BalanceQuantity",
                table: "FabricRequirements",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ColorCode",
                table: "FabricRequirements",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FabricCode",
                table: "FabricRequirements",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "LineNumber",
                table: "FabricRequirements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Fgpos_FactoryId",
                table: "Fgpos",
                column: "FactoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fgpos_Factories_FactoryId",
                table: "Fgpos",
                column: "FactoryId",
                principalTable: "Factories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
