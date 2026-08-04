using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddFabricPOFgpoAllocatedQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "FabricPOs");

            migrationBuilder.DropColumn(
                name: "Style",
                table: "FabricPOs");

            migrationBuilder.AddColumn<decimal>(
                name: "AllocatedQuantity",
                table: "FabricPOFgpos",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "FabricPOFgpos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "FabricPOFgpos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Style",
                table: "FabricPOFgpos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllocatedQuantity",
                table: "FabricPOFgpos");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "FabricPOFgpos");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "FabricPOFgpos");

            migrationBuilder.DropColumn(
                name: "Style",
                table: "FabricPOFgpos");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "FabricPOs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Style",
                table: "FabricPOs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
