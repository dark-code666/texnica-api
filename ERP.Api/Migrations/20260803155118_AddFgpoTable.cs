using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddFgpoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fgpos",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FGPONumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TemporaryNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Customer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Buyer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Brand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Style = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StyleDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Season = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PurchaseOrder = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Color = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SizeRange = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OrderQuantity = table.Column<int>(type: "int", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Factory = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fgpos", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fgpos_FGPONumber",
                table: "Fgpos",
                column: "FGPONumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fgpos");
        }
    }
}
