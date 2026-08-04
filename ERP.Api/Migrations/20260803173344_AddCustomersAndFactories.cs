using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomersAndFactories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Customer",
                table: "Fgpos");

            migrationBuilder.DropColumn(
                name: "Factory",
                table: "Fgpos");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Fgpos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FactoryId",
                table: "Fgpos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Factories",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Contact = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factories", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fgpos_CustomerId",
                table: "Fgpos",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Fgpos_FactoryId",
                table: "Fgpos",
                column: "FactoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Name",
                table: "Customers",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Factories_Name",
                table: "Factories",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Fgpos_Customers_CustomerId",
                table: "Fgpos",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fgpos_Factories_FactoryId",
                table: "Fgpos",
                column: "FactoryId",
                principalTable: "Factories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fgpos_Customers_CustomerId",
                table: "Fgpos");

            migrationBuilder.DropForeignKey(
                name: "FK_Fgpos_Factories_FactoryId",
                table: "Fgpos");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Factories");

            migrationBuilder.DropIndex(
                name: "IX_Fgpos_CustomerId",
                table: "Fgpos");

            migrationBuilder.DropIndex(
                name: "IX_Fgpos_FactoryId",
                table: "Fgpos");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Fgpos");

            migrationBuilder.DropColumn(
                name: "FactoryId",
                table: "Fgpos");

            migrationBuilder.AddColumn<string>(
                name: "Customer",
                table: "Fgpos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Factory",
                table: "Fgpos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
