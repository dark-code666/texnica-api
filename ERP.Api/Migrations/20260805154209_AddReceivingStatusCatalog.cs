using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ERP.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddReceivingStatusCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CatalogValues",
                columns: new[] { "ID", "Active", "Type", "UpdatedAt", "Value" },
                values: new object[,]
                {
                    { 42, true, "ReceivingStatus", null, "Pending" },
                    { 43, true, "ReceivingStatus", null, "Partially Received" },
                    { 44, true, "ReceivingStatus", null, "Fully Received" },
                    { 45, true, "ReceivingStatus", null, "Quantity Difference" },
                    { 46, true, "ReceivingStatus", null, "Rejected" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CatalogValues",
                keyColumn: "ID",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "CatalogValues",
                keyColumn: "ID",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "CatalogValues",
                keyColumn: "ID",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "CatalogValues",
                keyColumn: "ID",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "CatalogValues",
                keyColumn: "ID",
                keyValue: 46);
        }
    }
}
